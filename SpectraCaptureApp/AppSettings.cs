using Splat;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpectraCaptureApp
{
    internal static class AppSettings
    {
        private static SettingsManager<UserSettings> SettingsManager = Locator.Current.GetService<SettingsManager<UserSettings>>();
        private static UserSettings Settings = SettingsManager.LoadSettings();

        public static int RetryAttempts
        {
            get
            {
                return Settings.FailedAttemptTries;
            }
            set
            {
                Settings.FailedAttemptTries = value;
                SettingsManager.SaveSettings(Settings);
            }
        }

        public static string SpectrumSaveDirectory
        {
            get
            {
                return Settings.SpectrumSaveDirectory;
            }
            set
            {
                Settings.SpectrumSaveDirectory = value;
                SettingsManager.SaveSettings(Settings);
            }
        }
    }
}
