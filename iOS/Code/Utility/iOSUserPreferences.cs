using System;
using Foundation;

namespace actchargers.iOS
{
    public class IOSUserPreferences : IUserPreferences
    {
        /// <summary>
        /// Sets the int.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        public void SetInt(string key, int value)
        {
            NSUserDefaults.StandardUserDefaults.SetInt(value, key);
        }

        /// <summary>
        /// Gets the int.
        /// </summary>
        /// <returns>The int.</returns>
        /// <param name="key">Key.</param>
        public int GetInt(string key)
        {
            int value = (int)NSUserDefaults.StandardUserDefaults.IntForKey(key);
            return value;
        }

        /// <summary>
        /// Sets the long.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        public void SetLong(string key, long value)
        {
            NSUserDefaults.StandardUserDefaults.SetInt((nint)value, key);
        }

        /// <summary>
        /// Gets the long.
        /// </summary>
        /// <returns>The long.</returns>
        /// <param name="key">Key.</param>
        public long GetLong(string key)
        {
            long value = NSUserDefaults.StandardUserDefaults.IntForKey(key);
            return value;
        }

        /// <summary>
        /// Sets the bool.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">If set to <c>true</c> value.</param>
        public void SetBool(string key, bool value)
        {
            NSUserDefaults.StandardUserDefaults.SetBool(value, key);
        }

        /// <summary>
        /// Gets the bool.
        /// </summary>
        /// <returns>true</returns>
        /// <c>false</c>
        /// <param name="key">Key.</param>
        public bool GetBool(string key)
        {
            bool value = NSUserDefaults.StandardUserDefaults.BoolForKey(key);
            return value;
        }

        /// <summary>
        /// Sets the string.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        public void SetString(string key, string value)
        {
            NSUserDefaults.StandardUserDefaults.SetString(value, key);
        }

        /// <summary>
        /// Gets the string.
        /// </summary>
        /// <returns>The string.</returns>
        /// <param name="key">Key.</param>
        public string GetString(string key)
        {
            string value = NSUserDefaults.StandardUserDefaults.StringForKey(key);
            return value;
        }

        public void Clear()
        {
            string domain = NSBundle.MainBundle.BundleIdentifier;
            NSUserDefaults.StandardUserDefaults.RemovePersistentDomain(domain);

            NSUserDefaults.StandardUserDefaults.Synchronize();
        }
    }
}