using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace actchargers
{// ToMcbConfigBatteryType

    public class ToMcbConfigBatteryType : Newtonsoft.Json.Converters.DateTimeConverterBase
    {

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            string v = (string)value;
            byte r = (byte)Array.BinarySearch(MCBConfig.batteryTypes, v);
            if (r > 2 || r < 0)
                r = 0;

            writer.WriteRawValue(r.ToString());

        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null) { return null; }
            byte r = (byte)reader.Value;
            if (r > 2 || r < 0)
                r = 0;
            return MCBConfig.batteryTypes[r];
        }
    }
}
