using System.Collections.Generic;

namespace Contracts.OneCallWeather
{
    public class CurrentWeatherContract : BaseContract
    {
        public double lat { get; set; }
        public double lon { get; set; }
        public string timezone { get; set; }
        public int timezone_offset { get; set; }
        public BaseWeatherContract.Current current { get; set; }
        public List<Minutely> minutely { get; set; }
        public List<Alert> alerts { get; set; }
    }
    public class Minutely
    {
        public int dt { get; set; }
        public int precipitation { get; set; }
    }

    public class Alert
    {
        public string sender_name { get; set; }
        public string @event { get; set; }
        public int start { get; set; }
        public int end { get; set; }
        public string description { get; set; }
        public List<string> tags { get; set; }
    }
}
