#region Using Directives
using System;
using Microsoft.Win32;
#endregion

namespace LeagueSpectator
{
    public static class RegistryUtilities
    {
        #region Methods
        public static String GetValue(RegistryHive hive, String key, String value)
        {
            RegistryView view;

            if (Environment.Is64BitOperatingSystem)
                view = RegistryView.Registry64;
            else
                view = RegistryView.Registry32;

            return GetValue(hive, view, key, value);
        }

        public static String GetValue(RegistryHive hive, RegistryView view, String key, String value)
        {
            try
            {
                using (RegistryKey baseKey = RegistryKey.OpenBaseKey(hive, view))
                using (RegistryKey subKey = baseKey.OpenSubKey(key))
                {
                    if (subKey == null)
                        return null;

                    String valueData = (String)subKey.GetValue(value);

                    if (String.IsNullOrWhiteSpace(valueData))
                        return null;

                    return valueData;
                }
            }
            catch
            {
                return null;
            }
        }
        #endregion
    }
}
