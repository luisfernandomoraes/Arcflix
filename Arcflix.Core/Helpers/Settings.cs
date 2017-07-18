// Helpers/Settings.cs
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace Arcflix.Helpers
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Api

        private const string SettingsApiKey = "ApiKey";
        private static readonly string SettingApiKeyDefault = "1f54bd990f1cdfb230adb312546d765d";
        public static string ApiKey
        {
            get
            {
                return AppSettings.GetValueOrDefault(SettingsApiKey, SettingApiKeyDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(SettingsApiKey, value);
            }
        }
        #endregion

        #region Setting Language

        private const string SettingsLanguageKey = "Language";
        private static readonly string SettingLanguageDefault = "en-US";


        public static string Language
        {
            get
            {
                return AppSettings.GetValueOrDefault(SettingsLanguageKey, SettingLanguageDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(SettingsLanguageKey, value);
            }
        }
        #endregion
    }
}