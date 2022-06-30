using System;
using Newtonsoft.Json;

namespace actchargers
{
    public class ToBoolean : Newtonsoft.Json.Converters.DateTimeConverterBase
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteRawValue((((byte)value != 0)).ToString().ToLower());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null)
            {
                return null;
            }
            return (byte)((bool)reader.Value ? 0x01 : 0x00);
        }
    }
}
