using Contracts.OneCallWeather;
using Entities.OneCallWeather;
using WeatherApp.Common.Services;

namespace Managers.OneCallWeather.Mapper
{
    public class OneCallWeatherMapper : IOneCallWeatherMapper
    {
        public CurrentWeatherEntity MapCurrentWeather(CurrentWeatherContract currentWeatherContract)
        {
            return new MapperService<CurrentWeatherContract, CurrentWeatherEntity>()
                .Map(currentWeatherContract);
        }

        public HistoricalWeatherEntity MapHistoricalWeather(HistoricalWeatherContract historicalWeatherContract)
        {
            return new MapperService<HistoricalWeatherContract, HistoricalWeatherEntity>()
                .Map(historicalWeatherContract);
        }
    }
}
