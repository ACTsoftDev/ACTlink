using System;
using Newtonsoft.Json;

namespace actchargers
{
    public class ToMcbConfigDaysMask : Newtonsoft.Json.Converters.DateTimeConverterBase
    {

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            DaysMask v = (DaysMask)value;


            writer.WriteRawValue((v.SerializeMe()).ToString());

        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null) { return null; }
            DaysMask d = new DaysMask();
            d.DeSerializeMe((byte)reader.Value);
            return d;
        }
    }
}
