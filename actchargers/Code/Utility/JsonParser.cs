using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace actchargers.Code.Utility
{
    public static class JsonParser
    {
        public static string SerializeObject(object source)
        {
            return JsonConvert.SerializeObject(source);
        }

        public static JObject SerializeToJsonObject(object source)
        {
            string jsonStr = JsonConvert.SerializeObject(source);
            return JObject.Parse(jsonStr);
        }

        public static JArray SerializeToJsonArray(object source)
        {
            string jsonStr = JsonConvert.SerializeObject(source);
            return JArray.Parse(jsonStr);
		}

		public static T DeserializeObject<T>(string jsonString)
		{
			return JsonConvert.DeserializeObject<T>(jsonString);
		}
    }
}
