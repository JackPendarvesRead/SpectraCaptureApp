using SpectraCaptureApp.Infrastructure;
using Splat;

namespace SpectraCaptureApp
{
    internal class UserSettings
    {
        public string SpectrumSaveDirectory { get; set; }
        public int RetryAttempts { get; set; }
        public int LoopPauseTime { get; set; }
        public bool AutomaticLoop { get; set; }
        public int CurrentAutoRefIncrement { get; set; }
        public AutoReferenceSettings AutoReferenceSetting { get; set; }
    }

    internal static class AppSettings
    {
        private static readonly SettingsManager<UserSettings> SettingsManager = Locator.Current.GetService<SettingsManager<UserSettings>>();
        private static readonly UserSettings Settings = SettingsManager.LoadSettings();

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

        public static bool AutomaticLoop
        {
            get
            {
                return Settings.AutomaticLoop;
            }
            set
            {
                Settings.AutomaticLoop = value;
                SettingsManager.SaveSettings(Settings);
            }
        }

        public static AutoReferenceSettings AutoReferenceSetting
        {
            get
            {
                return Settings.AutoReferenceSetting;
            }
            set
            {
                Settings.AutoReferenceSetting = value;
                SettingsManager.SaveSettings(Settings);
            }
        }

        public static int CurrentAutoRefIncrement
        {
            get
            {
                return Settings.CurrentAutoRefIncrement;
            }
            set
            {
                Settings.CurrentAutoRefIncrement = value;
                SettingsManager.SaveSettings(Settings);
            }
        }
    }

    
}
