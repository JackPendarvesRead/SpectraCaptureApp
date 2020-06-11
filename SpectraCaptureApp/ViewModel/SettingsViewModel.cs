using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using Splat;
using System.Windows;
using Serilog;

namespace SpectraCaptureApp.ViewModel
{
    public class SettingsViewModel : ReactiveObject, IRoutableViewModel
    {
        public string UrlPathSegment => "Settings";
        public IScreen HostScreen { get; }

        public ReactiveCommand<Unit, Unit> SaveDirectoryBrowseCommand { get; set; }

        private string saveDirectory;
        public string SaveDirectory
        {
            get => saveDirectory;
            set => this.RaiseAndSetIfChanged(ref saveDirectory, value);
        }

        private readonly SettingsManager<UserSettings> settingsManager;
        private readonly UserSettings appSettings;

        public SettingsViewModel(IScreen screen = null)
        {
            HostScreen = screen ?? Locator.Current.GetService<IScreen>();

            settingsManager = Locator.Current.GetService<SettingsManager<UserSettings>>();
            appSettings = settingsManager.LoadSettings() ?? new UserSettings();

            SaveDirectory = appSettings.SpectrumSaveDirectory ?? "<No Directory Set>";

            SaveDirectoryBrowseCommand = ReactiveCommand.Create(() => 
            {
                using var fbd = new System.Windows.Forms.FolderBrowserDialog();
                if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    SaveDirectory = fbd.SelectedPath;
                    appSettings.SpectrumSaveDirectory = SaveDirectory;
                    settingsManager.SaveSettings(appSettings);
                    Log.Debug("SaveDirector was set to - {SaveDirectory}", SaveDirectory);
                }
            });
            SaveDirectoryBrowseCommand.ThrownExceptions.Subscribe((error) =>
            {
                Log.Error(error, "Failed to set save directory");
                MessageBox.Show(error.Message,
                   "Set sample reference method failed",
                   MessageBoxButton.OK,
                   MessageBoxImage.Error);
            });
        }
    }
}
