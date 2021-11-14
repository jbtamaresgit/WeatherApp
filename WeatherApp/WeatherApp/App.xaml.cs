using APIService.OneCallWeather;
using Managers.Notes;
using Managers.OneCallWeather;
using Managers.OneCallWeather.Mapper;
using Prism;
using Prism.DryIoc;
using Prism.Ioc;
using Repository.Database;
using Repository.Repositories.Notes;
using System;
using WeatherApp.ViewModels;
using WeatherApp.Views;
using Xamarin.Forms;

namespace WeatherApp
{
    public partial class App : PrismApplication
    {
        public App(IPlatformInitializer platformInitializer) : base(platformInitializer)
        {
            InitializeComponent();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }


        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();

            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterSingleton<IOneCallWeatherMapper, OneCallWeatherMapper>();
            containerRegistry.RegisterSingleton<IOneCallManager, OneCallManager>();
            containerRegistry.RegisterSingleton<IOneCallApiService, OneCallApiService>();

            containerRegistry.RegisterForNavigation<AddNotesPage, AddNotesViewModel>();
            containerRegistry.RegisterSingleton<INotesManager, NotesManager>();
            containerRegistry.RegisterSingleton<IDatabaseService, DatabaseService>();
            containerRegistry.RegisterSingleton<INotesRepository, NotesRepository>();
        }

        protected override void OnInitialized()
        {
            NavigationService.NavigateAsync("MainPage");
        }
    }
}
