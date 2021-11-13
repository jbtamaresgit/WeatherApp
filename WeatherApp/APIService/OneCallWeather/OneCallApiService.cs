using Contracts.OneCallWeather;
using System.Threading.Tasks;
using WeatherApp.Common.Models.API;

namespace APIService.OneCallWeather
{
    public class OneCallApiService : BaseAPIService, IOneCallApiService
    {
        public async Task<CurrentWeatherContract> GetCurrentWeather(WeatherApiModel model)
        {
            string endPoint = $"onecall?lat={model.lat}&lon={model.lon}&appid={BaseAPIKey}";
            CurrentWeatherContract result = await GetAsync<CurrentWeatherContract>(endPoint);
            return result;
        }

        public async Task<HistoricalWeatherContract> GetHistoricalWeather(HistoricalWeatherApiModel model)
        {
            string endPoint = $"onecall/timemachine?lat={model.lat}&lon={model.lon}&dt={model.unixdate}&appid={BaseAPIKey}";
            HistoricalWeatherContract result = await GetAsync<HistoricalWeatherContract>(endPoint);
            return result;
        }
    }
}
