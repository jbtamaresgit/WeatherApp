using System;

namespace WeatherApp.Common.Models.API
{
    public class APIExceptionModel : Exception
    {
        public int StatusCode { get; set; }
        public string Content { get; set; }
    }
}
