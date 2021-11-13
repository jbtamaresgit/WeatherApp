using APIService.OneCallWeather;
using Managers.OneCallWeather;
using Managers.OneCallWeather.Mapper;
using Prism;
using Prism.DryIoc;
using Prism.Ioc;
using System;
using WeatherApp.ViewModels;
using WeatherApp.Views;

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
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterSingleton<IOneCallWeatherMapper, OneCallWeatherMapper>();
            containerRegistry.RegisterSingleton<IOneCallManager, OneCallManager>();
            containerRegistry.RegisterSingleton<IOneCallApiService, OneCallApiService>();
        }

        protected override void OnInitialized()
        {
            NavigationService.NavigateAsync("MainPage");
        }
    }
}
