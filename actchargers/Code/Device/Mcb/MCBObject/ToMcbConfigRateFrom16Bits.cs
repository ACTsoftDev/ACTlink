using System;
using Newtonsoft.Json;

namespace actchargers
{
    public class ToMcbConfigRateFrom16Bits :
    Newtonsoft.Json.Converters.DateTimeConverterBase
    {

        public override void WriteJson
        (JsonWriter writer, object value, JsonSerializer serializer)
        {
            UInt16 v = (UInt16)value;
            writer.WriteRawValue(((float)v / 100.0f).ToString());

        }

        public override object ReadJson
        (JsonReader reader, Type objectType,
         object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null) { return null; }
            return (UInt16)((float)reader.Value * 100);
        }
    }
}
