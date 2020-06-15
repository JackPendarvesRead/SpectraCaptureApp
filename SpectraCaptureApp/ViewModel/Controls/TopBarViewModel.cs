using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reactive;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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
        public Visibility NewScanButtonVisible 
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
        public ReactiveCommand<Unit, IRoutableViewModel> NewScanNavigateCommand { get; }
        #endregion

        public TopBarViewModel()
        {
            BaselineOkImageUri = ImagePaths.SolidGreen;
            SpectrometerConnectedImageUri = ImagePaths.SolidRed;

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
        }

        public void SetVisibilities(IRoutableViewModel currentViewModel)
        {
            switch (currentViewModel)
            {
                default:
                    NewScanButtonVisible = Visibility.Collapsed;
                    SettingsButtonVisible = Visibility.Visible;
                    BaselineOkImageVisible = Visibility.Collapsed;
                    SpectrometerConnectedImageVisible = Visibility.Visible;
                    break;

                case ScanReferenceViewModel _:
                    NewScanButtonVisible = Visibility.Visible;
                    SettingsButtonVisible = Visibility.Collapsed;
                    BaselineOkImageVisible = Visibility.Collapsed;
                    SpectrometerConnectedImageVisible = Visibility.Visible;
                    break;

                case ScanSubsampleViewModel _:
                    NewScanButtonVisible = Visibility.Visible;
                    SettingsButtonVisible = Visibility.Collapsed;
                    BaselineOkImageVisible = Visibility.Visible;
                    SpectrometerConnectedImageVisible = Visibility.Visible;
                    break;

                case SettingsViewModel _:
                    NewScanButtonVisible = Visibility.Visible;
                    SettingsButtonVisible = Visibility.Collapsed;
                    BaselineOkImageVisible = Visibility.Collapsed;
                    SpectrometerConnectedImageVisible = Visibility.Visible;
                    break;
            }
        }
    }
}
