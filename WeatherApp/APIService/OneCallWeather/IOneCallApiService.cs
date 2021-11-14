using Contracts.OneCallWeather;
using System.Threading.Tasks;
using WeatherApp.Common.Models.API;

namespace APIService.OneCallWeather
{
    public interface IOneCallApiService
    {
        Task<CurrentWeatherContract> GetCurrentWeather(WeatherApiModel model);
        Task<HistoricalWeatherContract> GetHistoricalWeather(HistoricalWeatherApiModel model);
    }
}
