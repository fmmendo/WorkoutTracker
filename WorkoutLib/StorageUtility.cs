using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutLib
{
    public static class StorageUtility
    {
        /// <summary>
        /// Writes given value to the given key in the settings.
        /// Overwrites existing key.
        /// </summary>
        /// <param name="key">Identifier key</param>
        /// <param name="value">Value to write</param>
        public static void WriteSetting(string key, object value)
        {
            if (!IsolatedStorageSettings.ApplicationSettings.Contains(key))
                IsolatedStorageSettings.ApplicationSettings.Add(key, value);
            else
                IsolatedStorageSettings.ApplicationSettings[key] = value;
        }

        /// <summary>
        /// Reads given key. Returns null if it doesn't exist
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object ReadSetting(string key)
        {
            if (!IsolatedStorageSettings.ApplicationSettings.Contains(key))
                return null;
            else return IsolatedStorageSettings.ApplicationSettings[key];
        }

        /// <summary>
        /// Clears all stored settings.
        /// </summary>
        public static void ClearSettings()
        {
            IsolatedStorageSettings.ApplicationSettings.Clear();
        }
    }
}
