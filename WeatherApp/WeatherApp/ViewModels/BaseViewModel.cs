using Prism.Mvvm;
using Prism.Navigation;

namespace WeatherApp.ViewModels
{
    public class BaseViewModel : BindableBase, INavigationAware
    {
        public readonly INavigationService NavigationService;

        public BaseViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        private NavigationMode _NavigationMode;
        public NavigationMode NavigationMode
        {
            get { return _NavigationMode; }
            set { SetProperty(ref _NavigationMode, value); }
        }

        private bool _IsBusy = false;
        public bool IsBusy
        {
            get { return _IsBusy; }
            set { SetProperty(ref _IsBusy, value); }
        }

        public virtual async void NextAsync(string Page, NavigationParameters Parameters = null)
        {
            await NavigationService.NavigateAsync(Page, Parameters);
        }

        public virtual async void GoBackAsync(NavigationParameters Parameters = null)
        {
            await NavigationService.GoBackAsync(parameters: Parameters);
        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {
            NavigationMode = parameters.GetNavigationMode();
        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {
            NavigationMode = parameters.GetNavigationMode();
        }
    }
}
