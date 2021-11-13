using Prism;
using Prism.DryIoc;
using Prism.Ioc;
using System;

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
            throw new NotImplementedException();
        }

        protected override void OnInitialized()
        {
            throw new NotImplementedException();
        }
    }
}
