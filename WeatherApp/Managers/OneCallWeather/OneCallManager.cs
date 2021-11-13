using APIService.OneCallWeather;
using Contracts.OneCallWeather;
using Entities.OneCallWeather;
using Managers.OneCallWeather.Mapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Common.Models.API;

namespace Managers.OneCallWeather
{
    class OneCallManager : BaseManager, IOneCallManager
    {
        readonly IOneCallApiService OneCallApiService;
        readonly IOneCallWeatherMapper OneCallWeatherMapper;
        public OneCallManager(IOneCallApiService oneCallApiService, IOneCallWeatherMapper oneCallWeatherMapper)
        {
            OneCallApiService = oneCallApiService;
            OneCallWeatherMapper = oneCallWeatherMapper;
        }

        public async Task<CurrentWeatherEntity> GetCurrentWeather(WeatherApiModel model)
        {
            CurrentWeatherEntity result = new CurrentWeatherEntity();

            if (IsNetworkAvailable())
            {
                try
                {
                    CurrentWeatherContract resultContract = await OneCallApiService.GetCurrentWeather(model);
                    result = OneCallWeatherMapper.MapCurrentWeather(resultContract);
                }
                catch (APIExceptionModel ex)
                {
                    result.StatusCode = ex.StatusCode;
                    result.Message = ex.Content;
                }

            }
            else
            {
                result.Message = "Network is not available";
                result.StatusCode = -1;
            }

            return result;
        }

        public async Task<HistoricalWeatherEntity> GetHistoricalWeather(HistoricalWeatherApiModel model)
        {
            HistoricalWeatherEntity result = new HistoricalWeatherEntity();

            if (IsNetworkAvailable())
            {
                try
                {
                    HistoricalWeatherContract resultContract = await OneCallApiService.GetHistoricalWeather(model);
                    result = OneCallWeatherMapper.MapHistoricalWeather(resultContract);
                }
                catch (APIExceptionModel ex)
                {
                    result.StatusCode = ex.StatusCode;
                    result.Message = ex.Content;
                }

            }
            else
            {
                result.Message = "Network is not available";
                result.StatusCode = -1;
            }

            return result;
        }
    }
}
