using Newtonsoft.Json;

namespace WeatherApp.Common.Services
{
    public class MapperService<T, K> : IMapperService<T, K> where T : class
                                                          where K : class
    {
        public K Map(T contract)
        {
            var SerializedObject = JsonConvert.SerializeObject(contract);
            K output = JsonConvert.DeserializeObject<K>(SerializedObject);
            return output;
        }

    }
}
