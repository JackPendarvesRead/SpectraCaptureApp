using ReactiveUI;
using System;
using System.Collections.Generic;
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
        //public string BaselineOkImageUri => "/Images/tick.png";
        public string BaselineOkImageUri => GetURIString("Images/tick.png");
        public string SpectrometerConnectedImageUri => "/Images/tick.png";
        //public ImageSource BaselineOkImageUri => new BitmapImage(new Uri("/Images/tick.png"));
        //public ImageSource SpectrometerConnectedImageUri => new BitmapImage(new Uri("/Images/tick.png"));

        private string GetURIString(string path)
        {
            return "pack://application:,,,/" + path;
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
            NewScanButtonVisible = Visibility.Visible;
            SettingsButtonVisible = Visibility.Visible;
            BaselineOkImageVisible = Visibility.Visible;
            SpectrometerConnectedImageVisible = Visibility.Visible;
        }

        public void SetVisibilities(IRoutableViewModel currentViewModel)
        {
            switch (currentViewModel)
            {
                default:
                    NewScanButtonVisible = Visibility.Visible;
                    SettingsButtonVisible = Visibility.Visible;
                    BaselineOkImageVisible = Visibility.Collapsed;
                    SpectrometerConnectedImageVisible = Visibility.Collapsed;
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
