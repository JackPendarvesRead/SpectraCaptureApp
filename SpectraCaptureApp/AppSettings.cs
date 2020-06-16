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

        public static string SpectrumSaveDirectory
        {
            get
            {
                return Settings.SpectrumSaveDirectory ?? "<No Directory Set>";
            }
            set
            {
                Settings.SpectrumSaveDirectory = value;
                SettingsManager.SaveSettings(Settings);
            }
        }

        public static int RetryAttempts
        {
            get
            {
                return Settings.RetryAttempts == default ? 3 : Settings.RetryAttempts;
            }
            set
            {
                Settings.RetryAttempts = value;
                SettingsManager.SaveSettings(Settings);
            }
        }

        public static int LoopPauseTime
        {
            get
            {
                return Settings.LoopPauseTime == default ? 3 : Settings.LoopPauseTime;
            }
            set
            {
                Settings.LoopPauseTime = value;
                SettingsManager.SaveSettings(Settings);
            }
        }
    }
}
