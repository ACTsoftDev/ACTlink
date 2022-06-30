using System;
using System.Collections.Generic;
using System.Linq;
using actchargers.Code.Utility;
using Newtonsoft.Json.Linq;

namespace actchargers
{
    public class ListToJson
    {
        public static JProperty SerializeToJsonProperty<T>(List<T> list)
        where T : DBModel
        {
            string tableName = GetTableName(typeof(T));
            var jArray = JsonParser.SerializeToJsonArray(list);

            JProperty jProperty = new JProperty(tableName, jArray);

            return jProperty;
        }

        static string GetTableName(Type type)
        {
            string fullName = type.ToString();
            string[] fullNameArray = fullName.Split('.');

            return fullNameArray.LastOrDefault();
        }
    }
}
