using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using Splat;
using System.Windows;
using Serilog;
using SpectraCaptureApp.ViewModel.Controls;
using SpectraCaptureApp.Model;

namespace SpectraCaptureApp.ViewModel
{
    public class SettingsViewModel : ReactiveObject, IRoutableViewModel
    {
        public string UrlPathSegment => "Settings";
        public IScreen HostScreen { get; }

        public ReactiveCommand<Unit, Unit> SaveDirectoryBrowseCommand { get; set; }
        public ReactiveCommand<Unit, IRoutableViewModel> BackCommand { get; set; }

        private string saveDirectory;
        public string SaveDirectory
        {
            get => saveDirectory;
            set => this.RaiseAndSetIfChanged(ref saveDirectory, value);
        }

        public NumberInputViewModel RetryAttempts { get; set; }

        public SettingsViewModel(IScreen screen = null)
        {
            HostScreen = screen ?? Locator.Current.GetService<IScreen>();

            BackCommand = ReactiveCommand.CreateFromObservable(()
                => HostScreen.Router.NavigateAndReset.Execute(new EnterSampleReferenceViewModel(new ScanCaptureModel(), HostScreen)));

            SaveDirectory = AppSettings.SpectrumSaveDirectory ?? "<No Directory Set>";
            var currentRetryValue = AppSettings.RetryAttempts == default ? 3 : AppSettings.RetryAttempts;
            RetryAttempts = new NumberInputViewModel("Retry Attempts = ", currentRetryValue, 1, 5);

            RetryAttempts.CurrentValue.WhenAnyValue(x => x).Subscribe((x) =>
            {
                AppSettings.RetryAttempts = x;
            });

            SaveDirectoryBrowseCommand = ReactiveCommand.Create(() => 
            {
                using var fbd = new System.Windows.Forms.FolderBrowserDialog();
                if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    SaveDirectory = fbd.SelectedPath;
                    AppSettings.SpectrumSaveDirectory = SaveDirectory;
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
