using Managers.Notes;
using Managers.OneCallWeather;
using MvvmHelpers;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherApp.Common.Helpers;
using WeatherApp.Common.Models.API;
using WeatherApp.Common.Models.Collections;
using Xamarin.Essentials;
using Xamarin.Plugin.Calendar.Models;

namespace WeatherApp.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        readonly IOneCallManager OneCallManager;
        readonly INotesManager NotesManager;

        public MainPageViewModel(INavigationService navigationService, IOneCallManager oneCallManager,
            INotesManager notesManager) : base(navigationService)
        {
            OneCallManager = oneCallManager;
            NotesManager = notesManager;
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

        private string _DayName;
        public string DayName
        {
            get { return _DayName; }
            set { SetProperty(ref _DayName, value); }
        }

        private int _DayNumber;
        public int DayNumber
        {
            get { return _DayNumber; }
            set { SetProperty(ref _DayNumber, value); }
        }

        private string _Time;
        public string Time
        {
            get { return _Time; }
            set { SetProperty(ref _Time, value); }
        }

        private DateTime _DateToday = DateTime.Now.Date;
        public DateTime DateToday
        {
            get { return _DateToday; }
            set { SetProperty(ref _DateToday, value); }
        }

        private ObservableRangeCollection<HistoryWeatherModel> _WeatherHistoryList;
        public ObservableRangeCollection<HistoryWeatherModel> WeatherHistoryList
        {
            get { return _WeatherHistoryList; }
            set { SetProperty(ref _WeatherHistoryList, value); }
        }

        private EventCollection _Events;
        public EventCollection Events
        {
            get { return _Events; }
            set { SetProperty(ref _Events, value); }
        }

        private DelegateCommand _AddNotesCommand;
        public DelegateCommand AddNotesCommand =>
            _AddNotesCommand ?? (_AddNotesCommand = new DelegateCommand(ExecuteAddNotesCommand));

        void ExecuteAddNotesCommand()
        {
            base.NextAsync("AddNotesPage");
        }


        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            //fetch current location
            var location = await Geolocation.GetLocationAsync();
            GetCurrentDay();

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

                //fetch previous 4 days, current forecast is excluded
                int prevDays = -4;
                WeatherHistoryList = new ObservableRangeCollection<HistoryWeatherModel>();
                for (int x = 0; x < 4; x++)
                { 
                    await Get5DaysHistoricalWeather(historicalWeatherApiModel, DateTime.Today.AddDays(prevDays++));
                    _ = WeatherHistoryList.Reverse();
                }
            }

            GetNotes();

        }

        private void GetNotes()
        {
            //fetch current month notes 
            var result = NotesManager.GetCurrentMonthNotes(DateTime.Now)
                .OrderBy(x => x.Day);

            if (result != null && result.Count() > 0)
            {
                Events = new EventCollection();
                foreach (var item in result)
                {
                    var itemDate = new DateTime(item.Year, item.Month, item.Day);
                    Events.Add(itemDate, new List<EventModel>
                    {
                        new EventModel
                        {
                            Title = item.Title,
                            Content = item.Content
                        }
                    });
                }
            }
        }

        //fetch current day
        private void GetCurrentDay()
        {

            DayName = $"{DateTime.Now.DayOfWeek}, ";
            DayNumber = DateTime.Now.Day;
            Time = DateTime.Now.ToString("H:mm");
        }

        
        //get 5 days historical weather
        private async Task Get5DaysHistoricalWeather(HistoricalWeatherApiModel historicalWeatherApiModel, DateTime historyDay)
        {
            try
            {
                IsBusy = true;
                historicalWeatherApiModel.unixdate = DateToUnixConverter.GetUnixDateTime(historyDay);
                var result = await OneCallManager.GetHistoricalWeather(historicalWeatherApiModel);
                //get the current weather
                var getCurrentWeather = result.current.weather.FirstOrDefault();
                //convert kelvin to celsius
                var TempCelsius = TemperatureConverter.KelvinToCelsius(result.current.temp);
                WeatherHistoryList.Add(new HistoryWeatherModel
                {
                    DayName = $"{historyDay.DayOfWeek}",
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
                var TempCelsius = TemperatureConverter.KelvinToCelsius(result.current.temp);
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
