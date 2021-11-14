using System.Collections.Generic;
using System.Net.Http;

namespace APIService
{
    public static class HttpClientExtension
    {
        public static HttpClient AddHeadersToClient(this HttpClient client, Dictionary<string, string> headers)
        {
            foreach (var item in headers)
            {
                client.DefaultRequestHeaders.Add(item.Key, item.Value);
            }

            return client;
        }
    }
}
