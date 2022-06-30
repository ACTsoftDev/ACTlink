using System;

namespace actchargers
{
    public static class TimeToTextHelper
    {
        public static string GetUploadTimeText()
        {
            long time = GetLastUploadDate();

            return GetTimeText(time);
        }

        public static string GetSynchSiteTimeText()
        {
            long time = GetLastSynchSiteDate();

            return GetTimeText(time);
        }

        static string GetTimeText(long time)
        {
            if (time <= 0)
            {
                return AppResources.never;
            }
            else
            {
                return ParseToText(time);
            }
        }

        static long GetLastUploadDate()
        {
            return DbSingleton
                .DBManagerServiceInstance
                .GetGenericObjectsLoader()
                .GetLastUploadDate();
        }

        static long GetLastSynchSiteDate()
        {
            return DbSingleton
                .DBManagerServiceInstance
                .GetGenericObjectsLoader()
                .GetLastSynchSiteDate();
        }

        static string ParseToText(long time)
        {
            try
            {
                return TryParseToText(time);
            }
            catch (Exception)
            {
                return AppResources.never;
            }
        }

        static string TryParseToText(long time)
        {
            DateTime data = new DateTime(time);
            if (IsToday(data))
            {
                return data.ToString("hh:mm tt");
            }
            else
            {
                return data.ToString("yyyy MMMMM dd");
            }
        }

        static bool IsToday(DateTime data)
        {
            return data.Date == DateTime.Today;
        }
    }
}
