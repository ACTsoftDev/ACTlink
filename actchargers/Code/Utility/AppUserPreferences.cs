using MvvmCross.Platform;

namespace actchargers
{
    public static class AppUserPreferences
    {
        static IUserPreferences GetInstance()
        {
            return Mvx.Resolve<IUserPreferences>();
        }

        public static void SetInt(string key, int value)
        {
            GetInstance().SetInt(key, value);
        }

        public static int GetInt(string key)
        {
            return GetInstance().GetInt(key);
        }

        public static void SetLong(string key, long value)
        {
            GetInstance().SetLong(key, value);
        }

        public static long GetLong(string key)
        {
            return GetInstance().GetLong(key);
        }

        public static void SetBool(string key, bool value)
        {
            GetInstance().SetBool(key, value);
        }

        public static bool GetBool(string key)
        {
            return GetInstance().GetBool(key);
        }

        public static void SetString(string key, string value)
        {
            GetInstance().SetString(key, value);
        }

        public static string GetString(string key)
        {
            return GetInstance().GetString(key);
        }

        public static void Clear()
        {
            GetInstance().Clear();
        }
    }
}
