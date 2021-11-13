using Managers.OneCallWeather;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherApp.Common.Helpers;
using WeatherApp.Common.Models.API;
using WeatherApp.Common.Models.Collections;
using Xamarin.Essentials;

namespace WeatherApp.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        readonly IOneCallManager OneCallManager;

        public MainPageViewModel(INavigationService navigationService, IOneCallManager oneCallManager) : base(navigationService)
        {
            OneCallManager = oneCallManager;
        }

        private string _Humidity;
        public string Humidity
        {
            get { return _Humidity; }
            set { SetProperty(ref _Humidity, value); }
        }

        private string _Weather;
        public string Weather
        {
            get { return _Weather; }
            set { SetProperty(ref _Weather, value); }
        }

        private string _Temperature;
        public string Temperature
        {
            get { return _Temperature; }
            set { SetProperty(ref _Temperature, value); }
        }

        private List<HistoryWeatherModel> _WeatherHistoryList;
        public List<HistoryWeatherModel> WeatherHistoryList
        {
            get { return _WeatherHistoryList; }
            set { SetProperty(ref _WeatherHistoryList, value); }
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            var location = await Geolocation.GetLocationAsync();

            if(location != null)
            {
                WeatherApiModel weatherApiModel = new WeatherApiModel
                {
                    lat = location.Latitude,
                    lon = location.Longitude
                };
                await GetCurrentWeather(weatherApiModel);

                HistoricalWeatherApiModel historicalWeatherApiModel = new HistoricalWeatherApiModel
                {
                    lat = weatherApiModel.lat,
                    lon = weatherApiModel.lon
                };

                for (int x=0; x<5; x++)
                {
                    historicalWeatherApiModel.unixdate = DateToUnixConverter.GetUnixDateTime(DateTime.Now.Date.AddDays(x--));
                    await Get5DaysHistoricalWeather(historicalWeatherApiModel);
                }
            }
        }

        
        //get 5 days historical weather
        private async Task Get5DaysHistoricalWeather(HistoricalWeatherApiModel historicalWeatherApiModel)
        {
            try
            {
                IsBusy = true;
                var result = await OneCallManager.GetHistoricalWeather(historicalWeatherApiModel);
                //get the current weather
                var getCurrentWeather = result.current.weather.FirstOrDefault();
                //convert fahrenheit to celsius
                var TempCelsius = TemperatureConverter.FahrenheitToCelsius(result.current.temp);
                WeatherHistoryList.Add(new HistoryWeatherModel
                {
                    Temperature = $"{TempCelsius}°",
                    Weather = $"{getCurrentWeather.main}"
                });

            }
            finally
            {
                IsBusy = false;
            }
        }

        //get current weather
        private async Task GetCurrentWeather(WeatherApiModel weatherApiModel)
        {
            try
            {
                IsBusy = true;
                //fetch results from the api
                var result = await OneCallManager.GetCurrentWeather(weatherApiModel);
                //get the current weather
                var getCurrentWeather = result.current.weather.FirstOrDefault();
                //convert fahrenheit to celsius
                var TempCelsius = TemperatureConverter.FahrenheitToCelsius(result.current.temp);
                Humidity = $"{result.current.humidity}% ";
                Weather = getCurrentWeather.main;
                Temperature = $"{TempCelsius}°";

            }
            finally
            {
                IsBusy = false;
            }
        }

    }
}
