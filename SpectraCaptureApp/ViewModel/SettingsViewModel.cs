using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using System.Windows.Forms;
using Splat;

namespace SpectraCaptureApp.ViewModel
{
    public class SettingsViewModel : ReactiveObject, IRoutableViewModel
    {
        public string UrlPathSegment => "Settings";
        public IScreen HostScreen { get; }

        public ReactiveCommand<Unit, Unit> SaveDirectoryBrowseCommand { get; set; }

        public string SaveDirectory { get; set; }

        private readonly SettingsManager<UserSettings> settingsManager;
        private readonly UserSettings appSettings;

        public SettingsViewModel(IScreen screen = null)
        {
            HostScreen = screen ?? Locator.Current.GetService<IScreen>();
            ReactiveCommand.Create(SaveDirectoryBrowseCommandImpl);

            settingsManager = Locator.Current.GetService<SettingsManager<UserSettings>>();
            appSettings = settingsManager.LoadSettings() ?? new UserSettings();

            SaveDirectoryBrowseCommand = ReactiveCommand.Create(SaveDirectoryBrowseCommandImpl);
        }

        private void SaveDirectoryBrowseCommandImpl()
        {
            try
            {
                using var fbd = new FolderBrowserDialog();
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    SaveDirectory = fbd.SelectedPath;
                    appSettings.SpectrumSaveDirectory = SaveDirectory;
                    settingsManager.SaveSettings(appSettings);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
