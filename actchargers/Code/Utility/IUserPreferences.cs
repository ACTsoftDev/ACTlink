namespace actchargers
{
    public interface IUserPreferences
    {
        /// <summary>
        /// Sets the int.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        void SetInt(string key, int value);

        /// <summary>
        /// Gets the int.
        /// </summary>
        /// <returns>The int.</returns>
        /// <param name="key">Key.</param>
        int GetInt(string key);

        /// <summary>
        /// Sets the long.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        void SetLong(string key, long value);

        /// <summary>
        /// Gets the long.
        /// </summary>
        /// <returns>The long.</returns>
        /// <param name="key">Key.</param>
        long GetLong(string key);

        /// <summary>
        /// Sets the bool.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">If set to <c>true</c> value.</param>
        void SetBool(string key, bool value);

        /// <summary>
        /// Gets the bool.
        /// </summary>
        /// <returns><c>true</c>, if bool was gotten, <c>false</c> otherwise.</returns>
        /// <param name="key">Key.</param>
        bool GetBool(string key);

        /// <summary>
        /// Sets the string.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        void SetString(string key, string value);

        /// <summary>
        /// Gets the string.
        /// </summary>
        /// <returns>The string.</returns>
        /// <param name="key">Key.</param>
        string GetString(string key);

        void Clear();
    }
}
