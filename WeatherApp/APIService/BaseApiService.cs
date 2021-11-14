using Contracts;
using Newtonsoft.Json;
using Polly;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Common.Models.API;

namespace APIService
{
    public abstract class BaseAPIService
    {
        protected HttpClient HttpClient;
        private HttpResponseMessage HttpResponseMessage;
        private Stream JsonStreamResponse = null;
        protected string BaseAPIKey = "";

#if DEBUG
        private static readonly string WeatherAPIBaseAddress = @"https://api.openweathermap.org/data/2.5/";
#else
        public static readonly string WeatherAPIBaseAddress = @"https://api.openweathermap.org/data/2.5/"; 
#endif
        public static string AccessToken { get; set; }
        private readonly Uri WeatherAPIBaseAddressUri;
        private BaseContract BaseContract;

        protected BaseAPIService(Action<HttpClient> httpClientModifier = null)
        {
            HttpClient = new HttpClient();
            HttpResponseMessage = new HttpResponseMessage();
            WeatherAPIBaseAddressUri = new Uri(WeatherAPIBaseAddress);
            this.HttpClient.BaseAddress = WeatherAPIBaseAddressUri;
            this.HttpClient.Timeout = TimeSpan.FromSeconds(60);

            httpClientModifier?.Invoke(this.HttpClient as HttpClient);
        }

        internal enum RequestType
        {
            Delete,
            Get,
            Post,
            Put
        }

        protected void AddTokenToHeader()
        {
            if (!string.IsNullOrEmpty(AccessToken) && !HttpClient.DefaultRequestHeaders.Contains("APIKey"))
            {
                this.HttpClient.DefaultRequestHeaders.Add("APIKey", AccessToken);
                //HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("APIKey", AccessToken);
            }
        }

        protected Task<T> GetAsync<T>(string requestUri, Dictionary<string, string> headerValues = null)
        {
            if (headerValues != null)
            {
                return SendWithRetryAsync<T>(RequestType.Get, requestUri, headerValues: headerValues);
            }
            else
            {
                return SendWithRetryAsync<T>(RequestType.Get, requestUri);
            }

        }

        protected Task<T> DeleteAsync<T>(string requestUri)
        {
            return SendWithRetryAsync<T>(RequestType.Delete, requestUri);
        }

        protected Task<T> PutAsync<T, K>(string requestUri, K obj) // where object
        {
            string jsonRequest = !obj.Equals(default(K)) ? JsonConvert.SerializeObject(obj) : null;
            return SendWithRetryAsync<T>(RequestType.Put, requestUri, jsonRequest);
        }

        protected Task<T> PostAsync<T, K>(string requestUri, K obj) // where object
        {
            string jsonRequest = !obj.Equals(default(K)) ? JsonConvert.SerializeObject(obj) : null;
            return SendWithRetryAsync<T>(RequestType.Post, requestUri, jsonRequest);
        }

        async Task<T> SendWithRetryAsync<T>(RequestType requestType, string requestUri, string jsonRequest = null, Dictionary<string, string> headerValues = null)
        {
            T result = default;

            result = await Policy
                .Handle<WebException>()
                .WaitAndRetryAsync(5, retryAttempt =>
                                   TimeSpan.FromMilliseconds((200 * retryAttempt)),
                    (exception, timeSpan, context) =>
                    {
                        Debug.WriteLine(exception.ToString());
                    }
                )
                .ExecuteAsync(async () => { return await SendAsync<T>(requestType, requestUri, jsonRequest); });

            return result;
        }

        async Task<T> SendAsync<T>(RequestType requestType, string requestUri, string jsonRequest = null, Dictionary<string, string> headerValues = null)
        {
            //T result = default;

            HttpContent content = null;

            if (jsonRequest != null)
            {
                content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            }

            try
            {

                AddTokenToHeader();

                if (headerValues != null)
                {
                    HttpClient.AddHeadersToClient(headerValues);
                }

                switch (requestType)
                {
                    case RequestType.Get:
                        HttpResponseMessage = await HttpClient.GetAsync(requestUri).ConfigureAwait(false);
                        break;
                    case RequestType.Post:
                        HttpResponseMessage = await HttpClient.PostAsync(requestUri, content).ConfigureAwait(false);
                        break;
                    case RequestType.Delete:
                        HttpResponseMessage = await HttpClient.DeleteAsync(requestUri).ConfigureAwait(false);
                        break;
                    case RequestType.Put:
                        HttpResponseMessage = await HttpClient.PutAsync(requestUri, content).ConfigureAwait(false);
                        break;
                    default:
                        throw new Exception("Not a valid request type");
                }

                if (HttpResponseMessage.Content.Headers.ContentType.MediaType == "application/pdf" && HttpResponseMessage.IsSuccessStatusCode)
                {
                    HttpResponseMessage.Content = new StringContent("{\"Message\":\"OK\",\"StatusCode\":0}");
                }
                else if (HttpResponseMessage.Content.Headers.ContentLength == 0)
                {
                    HttpResponseMessage.Content = new StringContent("{\"Message\":\"OK\",\"StatusCode\":0}");
                }
                else if (HttpResponseMessage.StatusCode == HttpStatusCode.InternalServerError) //TODO:Change implementation of InternalServerError
                {
                    HttpResponseMessage.Content = new StringContent("{\"Message\":\"InternalServerError\",\"StatusCode\":100}");
                }
                else if (HttpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
                {
                    HttpResponseMessage.Content = new StringContent("{\"Message\":\"OK\",\"StatusCode\":401}");
                }
                else if (HttpResponseMessage.StatusCode == HttpStatusCode.BadRequest)
                {
                    HttpResponseMessage.Content = new StringContent("{\"Message\":\"BadRequest\",\"StatusCode\":400}");
                }
            }
            catch (TaskCanceledException ex)
            {
                BaseContract = new BaseContract
                {
                    Message = ex.ToString()
                };

                HttpResponseMessage.Content = new StringContent(JsonConvert.SerializeObject(BaseContract));
            }
            catch (HttpRequestException ex)
            {
                BaseContract = new BaseContract
                {
                    Message = ex.ToString()
                };

                HttpResponseMessage.Content = new StringContent(JsonConvert.SerializeObject(BaseContract));
            }
            catch (Exception ex)
            {
                BaseContract = new BaseContract
                {
                    Message = ex.ToString()
                };

                HttpResponseMessage.Content = new StringContent(JsonConvert.SerializeObject(BaseContract));
            }
            finally
            {
                if (content != null)
                {
                    content.Dispose();
                }
            }

            if (HttpResponseMessage != null)
            {
                JsonStreamResponse = await HttpResponseMessage.Content.ReadAsStreamAsync().ConfigureAwait(false);
            }

            if (HttpResponseMessage.IsSuccessStatusCode)
            {
                T deserializedJSON = Deserializer.DeserializeJSONFromStream<T>(JsonStreamResponse);
                return deserializedJSON;
            }

            //if(HttpResponseMessage.IsSuccessStatusCode == false)
            //{
            //    throw new 
            //}

            //if (!string.IsNullOrEmpty(JsonResponse))
            //{
            //    result = JsonConvert.DeserializeObject<T>(JsonResponse, JsonSettingsExtension.JsonSerializerSettings());
            //}

            throw new APIExceptionModel
            {
                StatusCode = (int)HttpResponseMessage.StatusCode,
                Content = await StreamAsync(JsonStreamResponse)
            };
        }

        private static async Task<string> StreamAsync(Stream streamObj)
        {
            string content = string.Empty;

            if (streamObj != null)
            {
                using (StreamReader sr = new StreamReader(streamObj))
                {
                    content = await sr.ReadToEndAsync();
                }
            }

            return content;
        }
    }
}
