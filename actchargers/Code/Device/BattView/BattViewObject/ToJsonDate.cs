using System;
using Newtonsoft.Json;

namespace actchargers
{
    public class ToJsonDate : Newtonsoft.Json.Converters.DateTimeConverterBase
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            DateTime v = (DateTime)value;

            writer.WriteRawValue(v.ToString("yyyy-MM-dd hh:mm:ss"));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null)
            {
                return null;
            }
            return DateTime.Parse((string)reader.Value);
        }
    }
}
