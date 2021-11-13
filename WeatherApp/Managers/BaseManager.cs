using WeatherApp.Common.Extension;

namespace Managers
{
    public class BaseManager : IBaseManager
    {
        public bool IsNetworkAvailable()
        {
            return NetworkConnectivityExtension.CheckConnection();
        }
    }
}
