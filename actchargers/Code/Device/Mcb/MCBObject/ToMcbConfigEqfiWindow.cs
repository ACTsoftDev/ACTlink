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
{
    public class ToMcbConfigEqfiWindow : Newtonsoft.Json.Converters.DateTimeConverterBase
    {

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            string v = (string)value;

            Match match;
            ushort st;
            match = Regex.Match(v, @"^(\d{1,2}){1}:(\d{1,2}){1}$", RegexOptions.IgnoreCase);
            st = (ushort)(ushort.Parse(match.Groups[1].Value) * 60 + ushort.Parse(match.Groups[2].Value));
            writer.WriteRawValue(((UInt32)st * 60).ToString());

        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null) { return null; }
            UInt32 st = (UInt32)reader.Value;
            st /= (60 * 15);
            string outp = String.Format("{0:00}:{1:00}", st / 60, (st % 60));
            return outp;
        }
    }
}
