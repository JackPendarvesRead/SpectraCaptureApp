using ReactiveUI;
using System;
using System.Reactive;
using Splat;
using System.Windows;
using Serilog;
using SpectraCaptureApp.ViewModel.Controls;
using SpectraCaptureApp.Model;
using SpectraCaptureApp.Infrastructure;
using System.IO;
using System.Diagnostics;

namespace SpectraCaptureApp.ViewModel
{
    public class SettingsViewModel : ReactiveObject, IRoutableViewModel
    {
        public string UrlPathSegment => "Settings";
        public IScreen HostScreen { get; }

        public ReactiveCommand<Unit, Unit> SaveDirectoryBrowseCommand { get; set; }
        public ReactiveCommand<Unit, IRoutableViewModel> BackCommand { get; set; }
        public ReactiveCommand<Unit, Unit> RefreshIncrementCommand { get; set; }
        public ReactiveCommand<Unit, Unit> ViewLogsCommand { get; set; }

        private string saveDirectory;
        public string SaveDirectory
        {
            get => saveDirectory;
            set => this.RaiseAndSetIfChanged(ref saveDirectory, value);
        }

        private bool automaticLoop;
        public bool AutomaticLoop
        {
            get => automaticLoop;
            set => this.RaiseAndSetIfChanged(ref automaticLoop, value);
        }

        public NumberInputViewModel RetryAttemptsViewModel { get; set; }
        public NumberInputViewModel LoopDelayViewModel { get; set; }


        private AutoReferenceSettings autoReferenceSetting;
        public AutoReferenceSettings AutoReferenceSetting 
        {
            get => autoReferenceSetting;
            set => this.RaiseAndSetIfChanged(ref autoReferenceSetting, value);
        }
        public Array AutoReferenceSettingsList => Enum.GetValues(typeof(AutoReferenceSettings));

        private int currentAutoIncrement;
        public int CurrentAutoIncrement
        {
            get => currentAutoIncrement;
            set => this.RaiseAndSetIfChanged(ref currentAutoIncrement, value);
        }

        public SettingsViewModel(IScreen screen = null)
        {
            HostScreen = screen ?? Locator.Current.GetService<IScreen>();
            CurrentAutoIncrement = AppSettings.CurrentAutoRefIncrement;

            AutomaticLoop = AppSettings.AutomaticLoop;
            SaveDirectory = AppSettings.SpectrumSaveDirectory;
            AutoReferenceSetting = AppSettings.AutoReferenceSetting;
            RetryAttemptsViewModel = new NumberInputViewModel("Retry Attempts", AppSettings.RetryAttempts, 1, 5);
            LoopDelayViewModel = new NumberInputViewModel("Loop pause time (s)", AppSettings.LoopPauseTime, 1, 99);

            this.WhenAnyValue(vm => vm.RetryAttemptsViewModel.CurrentValue)
                .Subscribe(newValue =>
                {
                    AppSettings.RetryAttempts = newValue;
                });
            this.WhenAnyValue(vm => vm.LoopDelayViewModel.CurrentValue)
                .Subscribe(newValue =>
                {
                    AppSettings.LoopPauseTime = newValue;
                });
            this.WhenAnyValue(vm => vm.AutoReferenceSetting)
                .Subscribe((newValue) =>
                {
                    AppSettings.AutoReferenceSetting = newValue;
                });
            this.WhenAnyValue(vm => vm.AutomaticLoop)
                .Subscribe((newValue) =>
                {
                    AppSettings.AutomaticLoop = newValue;
                });


            BackCommand = ReactiveCommand.CreateFromObservable(()
                => HostScreen.Router.NavigateAndReset.Execute(new EnterSampleReferenceViewModel(new ScanCaptureModel(), HostScreen)));
            
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
                MessageBox.Show(
                    error.Message,                   
                    "Set sample reference method failed",                   
                    MessageBoxButton.OK,                   
                    MessageBoxImage.Error);
            });

            RefreshIncrementCommand = ReactiveCommand.Create(() => 
            { 
                AppSettings.CurrentAutoRefIncrement = 0;
                CurrentAutoIncrement = 0;
            });

            ViewLogsCommand = ReactiveCommand.Create(ViewLogsImpl);
            ViewLogsCommand.ThrownExceptions.Subscribe((ex) =>
            {
                MessageBox.Show(ex.Message);
            });
        }

        private void ViewLogsImpl()
        {
            var folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            if (Directory.Exists(folderPath))
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    Arguments = folderPath,
                    FileName = "explorer.exe"
                };
                Process.Start(startInfo);
            }
            else
            {
                Log.Warning("Could not find logs folder. Path={FolderPath}", folderPath);
                MessageBox.Show($"Could not find logs folder. Path={folderPath}");
            }

        }
    }
}
