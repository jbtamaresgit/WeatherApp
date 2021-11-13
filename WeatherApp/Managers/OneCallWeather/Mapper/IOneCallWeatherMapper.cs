using Contracts.OneCallWeather;
using Entities.OneCallWeather;

namespace Managers.OneCallWeather.Mapper
{
    public interface IOneCallWeatherMapper
    {
        CurrentWeatherEntity MapCurrentWeather(CurrentWeatherContract currentWeatherContract);
        HistoricalWeatherEntity MapHistoricalWeather(HistoricalWeatherContract historicalWeatherContract);
    }
}
