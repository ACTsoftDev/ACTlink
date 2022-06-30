using System;
using Newtonsoft.Json;

namespace actchargers
{
    public class ToTimeStamp : Newtonsoft.Json.Converters.DateTimeConverterBase
    {

        public override void WriteJson
        (JsonWriter writer, object value, JsonSerializer serializer)
        {
            DateTime v = (DateTime)value;
            writer.WriteRawValue
                  (StaticDataAndHelperFunctions.getZoneUnixTimeStampFromUTC
                   (0, v, false).ToString());

        }

        public override object ReadJson
        (JsonReader reader, Type objectType, object existingValue,
         JsonSerializer serializer)
        {
            if (reader.Value == null) { return null; }
            return StaticDataAndHelperFunctions.getZoneUnixTimeStampFromUTC
                                               (0, (UInt32)reader.Value, false);
        }
    }
}
