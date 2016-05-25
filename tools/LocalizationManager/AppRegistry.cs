using Microsoft.Win32;
using System;

namespace LocalizationManager {
    public enum RegistryKeys {
        SolutionPath,
        SelectedProject,
        SelectedDictionary
    }

    public class AppRegistry {
        const string MainKeyName = "SOFTWARE\\NT\\LocalizationManager";

        internal static string GetValue(RegistryKeys regKey) {
            RegistryKey mainKey = Registry.CurrentUser.CreateSubKey(MainKeyName);
            object value = mainKey.GetValue(regKey.ToString());

            if (value == null)
                return String.Empty;

            return (string)value;
        }

        internal static void SetValue(RegistryKeys regKey, string value) {
            RegistryKey mainKey = Registry.CurrentUser.CreateSubKey(MainKeyName, true);
            mainKey.SetValue(regKey.ToString(), value);
            mainKey.Flush();
            mainKey.Close();
        }
    }
}
