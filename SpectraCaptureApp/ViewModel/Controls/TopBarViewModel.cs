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
        public ImageSource BaselineOkImageUri => new BitmapImage(new Uri("/Images/tick.png"));
        public ImageSource SpectrometerConnectedImageUri => new BitmapImage(new Uri("/Images/tick.png"));
        public ImageSource BatteryImage => new BitmapImage(new Uri("/Images/100.png"));

        public Visibility HomeButtonVisible { get; set; }
        public Visibility SettingsButtonVisible { get; set; }
        public Visibility BatteryImageVisible { get; set; }
        public Visibility BaselineOkImageVisible { get; set; }
        public Visibility SpectrometerConnectedImageVisible { get; set; }

        public ReactiveCommand<Unit, IRoutableViewModel> SettingsNavigateCommand { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> HomeNavigateCommand { get; }

        public TopBarViewModel()
        {
            HomeButtonVisible = Visibility.Collapsed;
            SettingsButtonVisible = Visibility.Visible;
            BatteryImageVisible = Visibility.Visible;
            BaselineOkImageVisible = Visibility.Visible;
            SpectrometerConnectedImageVisible = Visibility.Visible;
        }
    }
}
