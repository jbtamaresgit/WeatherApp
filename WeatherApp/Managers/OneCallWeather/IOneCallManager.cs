using Entities.OneCallWeather;
using System.Threading.Tasks;
using WeatherApp.Common.Models.API;

namespace Managers.OneCallWeather
{
    public interface IOneCallManager
    {
        Task<CurrentWeatherEntity> GetCurrentWeather(WeatherApiModel model);
        Task<HistoricalWeatherEntity> GetHistoricalWeather(HistoricalWeatherApiModel model);
    }
}
