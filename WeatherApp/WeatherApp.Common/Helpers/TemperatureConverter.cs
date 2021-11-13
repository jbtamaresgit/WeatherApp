namespace WeatherApp.Common.Helpers
{
    public static class TemperatureConverter
    {
        public static double FahrenheitToCelsius(double temp)
        {
            return (5.0 / 9.0) * (temp - 32);
        }
    }
}
