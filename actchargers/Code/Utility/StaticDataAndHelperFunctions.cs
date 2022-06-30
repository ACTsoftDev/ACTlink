using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;

namespace actchargers
{
    public static class StaticDataAndHelperFunctions
    {
        public static DateTime lastCheckSession = DateTime.MinValue;

        public static T DeepClone<T>(T source)
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream())
            {
                serializer.WriteObject(ms, source);
                ms.Seek(0, SeekOrigin.Begin);
                return (T)serializer.ReadObject(ms);
            }
        }

        public static UInt32 getZoneUnixTimeStampFromUTC(byte id, UInt32 unixTimeStamp, bool dayLightSaving)
        {
            JsonZone zone = getZoneByID(id);

            if (zone.id == 0)
                return unixTimeStamp;
            unixTimeStamp = (UInt32)(unixTimeStamp + zone.base_utc);
            if (dayLightSaving)
            {
                for (int i = 0; i < 50; i++)
                {
                    if (unixTimeStamp > zone.changes_time[i])
                        unixTimeStamp = (UInt32)(unixTimeStamp + ((i % 2 == 0) ? 1 : -1) * zone.changes_value);
                }
            }
            return unixTimeStamp;
        }

        public static UInt32 getZoneUnixTimeStampFromUTC(byte id, DateTime timeInUTC, bool dayLightSaving)
        {
            JsonZone zone = getZoneByID(id);

            UInt32 unixTimeStamp = (UInt32)(timeInUTC - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
            if (zone.id == 0)
                return unixTimeStamp;
            unixTimeStamp = (UInt32)(unixTimeStamp + zone.base_utc);
            if (dayLightSaving)
            {
                for (int i = 0; i < 50; i++)
                {
                    if (unixTimeStamp > zone.changes_time[i])
                        unixTimeStamp = (UInt32)(unixTimeStamp + ((i % 2 == 0) ? 1 : -1) * zone.changes_value);
                }
            }
            return unixTimeStamp;
        }

        public static DateTime getZoneTimeFromUTC(byte id, DateTime timeInUTC, bool dayLightSaving)
        {
            JsonZone zone = getZoneByID(id);

            Int64 unixTimeStamp = (UInt32)(timeInUTC - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
            if (zone.id == 0)
                return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(unixTimeStamp);
            unixTimeStamp = unixTimeStamp + zone.base_utc;
            if (dayLightSaving)
            {
                for (int i = 0; i < 50; i++)
                {
                    if (unixTimeStamp > zone.changes_time[i])
                        unixTimeStamp = (UInt32)(unixTimeStamp + ((i % 2 == 0) ? 1 : -1) * zone.changes_value);
                }
            }
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(unixTimeStamp);
        }

        public static string cleanWifiChars(string inString)
        {
            string outString = "";
            foreach (char c in inString)
            {
                if (c == '\'' || c == '\"' || c == '\\' || c == ' ' || c == ',')
                {
                    //do i need to replace it?
                }
                else
                {
                    outString += c;
                }
            }
            return outString;
        }

        public static JsonZone getZoneByID(byte id)
        {
            JsonZone myZone = new JsonZone("", 0, 0);
            List<JsonZone> zonesList = GetZonesList();
            foreach (JsonZone t in zonesList)
            {
                if (t.id == id)
                {
                    myZone = t;
                    break;
                }
            }
            return myZone;
        }

        public static JsonZone getZoneByText(string text)
        {
            JsonZone myZone = new JsonZone("", 0, 0);
            List<JsonZone> zonesList = GetZonesList();
            foreach (JsonZone t in zonesList)
            {
                if (t.display_name == text)
                {
                    myZone = t;
                    break;
                }
            }
            return myZone;
        }

        public static List<object> GetZonesListAsObjects()
        {
            try
            {
                return TryGetZonesListAsObjects();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Logger.AddLog(true, "X116" + "Zone Load:" + ex);

                return new List<object>();
            }
        }

        static List<object> TryGetZonesListAsObjects()
        {
            List<object> objects = new List<object>();

            List<JsonZone> zones = GetZonesList();

            foreach (var item in zones)
                objects.Add(item);

            return objects;
        }

        public static List<JsonZone> GetZonesList()
        {
            try
            {
                return TryGetZonesList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Logger.AddLog(true, "X116" + "Zone Load:" + ex);

                return new List<JsonZone>();
            }
        }

        static List<JsonZone> TryGetZonesList()
        {
            List<JsonZone> zones = new List<JsonZone>();

            string jsonString = ReadTimeZonesFile();

            zones = Newtonsoft.Json.JsonConvert.DeserializeObject<List<JsonZone>>(jsonString);

            return zones;
        }

        static string ReadTimeZonesFile()
        {
            string jsonString;

            var assembly = typeof(StaticDataAndHelperFunctions).GetTypeInfo().Assembly;

            string embeddedResource = "actchargers.DefaultFirmwareFiles.zonesJson.txt";

            using (StreamReader stm = new StreamReader(assembly.GetManifestResourceStream(embeddedResource)))
            {
                if (stm == null)
                    throw new Exception(embeddedResource + " is not found in Embedded Resources.");

                jsonString = stm.ReadToEnd();
            }

            return jsonString;
        }

        public static bool ValidateLoginDate()
        {
            string loginDateString =
                DbSingleton.DBManagerServiceInstance
                           .GetGenericObjectsLoader()
                           .GetValueOrDefault(ACConstants.DB_LOGIN_DATE);

            DateTime loginDate = DateTime.UtcNow;
            if (DateTime.TryParse(loginDateString, out loginDate))
            {
                if (DateTime.UtcNow.AddDays(ACConstants.NO_OF_DAYS_APP_TIMEOUT)
                    > loginDate)
                {
                    return false;
                }
            }
            else
            {
                DbSingleton.DBManagerServiceInstance.GetGenericObjectsLoader()
                           .InsertOrUpdateUsingFields
                           (ACConstants.DB_LOGIN_DATE, DateTime.UtcNow.ToString());
            }
            return true;
        }
    }
}
