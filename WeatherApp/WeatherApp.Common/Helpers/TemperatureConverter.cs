using System;

namespace WeatherApp.Common.Helpers
{
    public static class TemperatureConverter
    {
        public static double KelvinToCelsius(double temp)
        {
            return Math.Round(temp - 273.15);
        }
    }
}
