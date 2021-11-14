using System;

namespace WeatherApp.Common.Helpers
{
    public static class DateToUnixConverter
    {
        public static int GetUnixDateTime(DateTime date)
        {
            return (int)Math.Truncate(date.ToUniversalTime().Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
        }
    }
}
