using Plugin.Connectivity;

namespace WeatherApp.Common.Extension
{
    public static class NetworkConnectivityExtension
    {
        public static bool CheckConnection()
        {
            if (CrossConnectivity.IsSupported)
            {
                return CrossConnectivity.Current.IsConnected;
            }

            return false;
        }
    }
}
