using ReactiveUI;
using SpectraCaptureApp.Infrastructure;
using SpectraCaptureApp.Model;
using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows;

namespace SpectraCaptureApp.ViewModel.Controls
{
    public class TopBarViewModel : ReactiveObject
    {
        #region ImageUri
        private string baselineOkImageUri;
        public string BaselineOkImageUri
        {
            get => baselineOkImageUri;
            set => this.RaiseAndSetIfChanged(ref baselineOkImageUri, value);
        }

        private bool baselineIsOk;
        public bool BaselineIsOk
        {
            get => baselineIsOk;
            set => this.RaiseAndSetIfChanged(ref baselineIsOk, value);
        }

        private string spectrometerConnectedImageUri;
        public string SpectrometerConnectedImageUri
        {
            get => spectrometerConnectedImageUri;
            set => this.RaiseAndSetIfChanged(ref spectrometerConnectedImageUri, value);
        }

        private bool spectrometerIsConnected;
        public bool SpectrometerIsConnected
        {
            get => spectrometerIsConnected;
            set => this.RaiseAndSetIfChanged(ref spectrometerIsConnected, value);
        }
        #endregion

        #region Visability
        private Visibility homeButtonVisible;
        public Visibility AbortButtonVisible 
        {
            get => homeButtonVisible;
            set => this.RaiseAndSetIfChanged(ref homeButtonVisible, value);
        }

        private Visibility settingsButtonVisible;
        public Visibility SettingsButtonVisible
        {
            get => settingsButtonVisible;
            set => this.RaiseAndSetIfChanged(ref settingsButtonVisible, value);
        }

        private Visibility baselineOkImageVisible;
        public Visibility BaselineOkImageVisible
        {
            get => baselineOkImageVisible;
            set => this.RaiseAndSetIfChanged(ref baselineOkImageVisible, value);
        }

        private Visibility spectrometerConnectedImageVisible;
        public Visibility SpectrometerConnectedImageVisible
        {
            get => spectrometerConnectedImageVisible;
            set => this.RaiseAndSetIfChanged(ref spectrometerConnectedImageVisible, value);
        }
        #endregion

        #region Commands
        public ReactiveCommand<Unit, IRoutableViewModel> SettingsNavigateCommand { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> AbortCommand { get; }
        #endregion

        private readonly IScreen HostScreen;

        public TopBarViewModel(IScreen screen)
        {
            HostScreen = screen;
            BaselineOkImageUri = ImagePaths.SolidGreen;
            SpectrometerConnectedImageUri = ImagePaths.SolidRed;

            SettingsNavigateCommand = ReactiveCommand.CreateFromObservable(()
                => HostScreen.Router.NavigateAndReset.Execute(new SettingsViewModel(HostScreen)));
            AbortCommand = ReactiveCommand.CreateFromObservable(()
                => HostScreen.Router.NavigateAndReset.Execute(new EnterSampleReferenceViewModel(new ScanCaptureModel(), HostScreen)));

            this.WhenAnyValue(vm => vm.SpectrometerIsConnected)
                .Subscribe(connected =>
                {
                    if (connected)
                    {
                        SpectrometerConnectedImageUri = ImagePaths.SolidGreen;
                    }
                    else
                    {
                        SpectrometerConnectedImageUri = ImagePaths.SolidRed;
                    }
                });

            this.WhenAnyValue(vm => vm.BaselineIsOk)
                .Subscribe(connected =>
                {
                    if (connected)
                    {
                        BaselineOkImageUri = ImagePaths.SolidGreen;
                    }
                    else
                    {
                        BaselineOkImageUri = ImagePaths.SolidRed;
                    }
                });

            HostScreen.Router.NavigationChanged.Subscribe(x =>
            {
                this.SetVisibilities(Observable.Latest(HostScreen.Router.CurrentViewModel).First());
            });
        }

        public void SetVisibilities(IRoutableViewModel currentViewModel)
        {
            switch (currentViewModel)
            {
                default:
                    AbortButtonVisible = Visibility.Collapsed;
                    SettingsButtonVisible = Visibility.Visible;
                    BaselineOkImageVisible = Visibility.Collapsed;
                    SpectrometerConnectedImageVisible = Visibility.Visible;
                    break;

                case ScanReferenceViewModel _:
                    AbortButtonVisible = Visibility.Visible;
                    SettingsButtonVisible = Visibility.Collapsed;
                    BaselineOkImageVisible = Visibility.Collapsed;
                    SpectrometerConnectedImageVisible = Visibility.Visible;
                    break;

                case ScanSubsampleViewModel _:
                    AbortButtonVisible = Visibility.Visible;
                    SettingsButtonVisible = Visibility.Collapsed;
                    BaselineOkImageVisible = Visibility.Visible;
                    SpectrometerConnectedImageVisible = Visibility.Visible;
                    break;

                case SettingsViewModel _:
                    AbortButtonVisible = Visibility.Collapsed;
                    SettingsButtonVisible = Visibility.Collapsed;
                    BaselineOkImageVisible = Visibility.Collapsed;
                    SpectrometerConnectedImageVisible = Visibility.Visible;
                    break;

                case ErrorContactViewModel _:
                    AbortButtonVisible = Visibility.Visible;
                    SettingsButtonVisible = Visibility.Collapsed;
                    BaselineOkImageVisible = Visibility.Collapsed;
                    SpectrometerConnectedImageVisible = Visibility.Collapsed;
                    break;
            }
        }
    }
}
