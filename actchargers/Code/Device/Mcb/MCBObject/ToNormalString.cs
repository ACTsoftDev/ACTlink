using System;
using Newtonsoft.Json;

namespace actchargers
{
    public class ToNormalString : Newtonsoft.Json.Converters.DateTimeConverterBase
    {
        public override void WriteJson
        (JsonWriter writer, object value, JsonSerializer serializer)
        {
            string v = (string)value;
            v = v.TrimEnd(new[] { '\0' });
            v = v.TrimEnd(new[] { ' ' });
            writer.WriteRawValue("\"" + v + "\"");

        }

        public override object ReadJson
        (JsonReader reader, Type objectType, object existingValue,
         JsonSerializer serializer)
        {
            if (reader.Value == null) { return null; }
            return (string)reader.Value;
        }
    }
}
