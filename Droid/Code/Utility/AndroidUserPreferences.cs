using Android.Content;

namespace actchargers.Droid
{
    public class AndroidUserPreferences : IUserPreferences
    {
        public void SetInt (string key, int value)
        {
			ISharedPreferences prefs = Android.App.Application.Context.GetSharedPreferences (ACConstants.USER_PREFS_FILE, FileCreationMode.Private);
            ISharedPreferencesEditor prefsEditor = prefs.Edit ();

            prefsEditor.PutInt (key, value);
            prefsEditor.Commit ();
        }

        public int GetInt (string key)
        {
            ISharedPreferences prefs = Android.App.Application.Context.GetSharedPreferences (ACConstants.USER_PREFS_FILE, FileCreationMode.Private);
            return prefs.GetInt (key, 0);
        }

        public void SetLong (string key, long value)
        {
            ISharedPreferences prefs = Android.App.Application.Context.GetSharedPreferences (ACConstants.USER_PREFS_FILE, FileCreationMode.Private);
            ISharedPreferencesEditor prefsEditor = prefs.Edit ();

            prefsEditor.PutLong (key, value);
            prefsEditor.Commit ();
        }

        public long GetLong (string key)
        {
            ISharedPreferences prefs = Android.App.Application.Context.GetSharedPreferences (ACConstants.USER_PREFS_FILE, FileCreationMode.Private);
            return prefs.GetLong (key, 0);
        }

        public void SetBool (string key, bool value)
        {
            ISharedPreferences prefs = Android.App.Application.Context.GetSharedPreferences (ACConstants.USER_PREFS_FILE, FileCreationMode.Private);
            ISharedPreferencesEditor prefsEditor = prefs.Edit ();

            prefsEditor.PutBoolean (key, value);
            prefsEditor.Commit ();
        }

        public bool GetBool (string key)
        {
            ISharedPreferences prefs = Android.App.Application.Context.GetSharedPreferences (ACConstants.USER_PREFS_FILE, FileCreationMode.Private);
            return prefs.GetBoolean (key, false);
        }

        public void SetString (string key, string value)
        {
            ISharedPreferences prefs = Android.App.Application.Context.GetSharedPreferences (ACConstants.USER_PREFS_FILE, FileCreationMode.Private);
            ISharedPreferencesEditor prefsEditor = prefs.Edit ();

            prefsEditor.PutString (key, value);
            prefsEditor.Commit ();
        }

        public string GetString (string key)
        {
            ISharedPreferences prefs = Android.App.Application.Context.GetSharedPreferences (ACConstants.USER_PREFS_FILE, FileCreationMode.Private);
            return prefs.GetString (key, "");
        }

        public void Clear()
        {
            ISharedPreferences prefs = Android.App.Application.Context.GetSharedPreferences(ACConstants.USER_PREFS_FILE, FileCreationMode.Private);
            ISharedPreferencesEditor prefsEditor = prefs.Edit();

            prefsEditor.Clear();
            prefsEditor.Commit();
        }
    }
}

