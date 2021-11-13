using Newtonsoft.Json;
using System.IO;

namespace APIService
{
    public static class Deserializer
    {
        public static T DeserializeJSONFromStream<T>(Stream streamObj)
        {
            if (streamObj == null || streamObj.CanRead == false)
            {
                return default;
            }

            using (StreamReader sr = new StreamReader(streamObj))
            using (JsonTextReader jtr = new JsonTextReader(sr))
            {
                JsonSerializer jsonSerializer = new JsonSerializer();
                return jsonSerializer.Deserialize<T>(jtr);
            }
        }
    }
}
