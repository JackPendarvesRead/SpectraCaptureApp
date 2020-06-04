using SpectraCaptureApp.ViewModel;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SpectraCaptureApp.ViewModel.Controls;

namespace SpectraCaptureApp.View.Controls
{
    /// <summary>
    /// Interaction logic for FileBrowse.xaml
    /// </summary>
    public partial class TopBar : ReactiveUserControl<TopBarViewModel>
    {
        public TopBar()
        {
            InitializeComponent();
            this.WhenActivated(disposables =>
            {
                //this.OneWayBind(ViewModel, vm => vm.BatteryImage, view => view.BatteryImage.Source).DisposeWith(disposables);
                //this.OneWayBind(ViewModel, vm => vm.SpectrometerConnectedImageUri, view => view.SpectrometerConnectedImage.Source).DisposeWith(disposables);
                //this.OneWayBind(ViewModel, vm => vm.BaselineOkImageUri, view => view.BaselineOkImage.Source).DisposeWith(disposables);

                this.Bind(ViewModel, vm => vm.HomeButtonVisible, view => view.HomeButton.Visibility, VMToView ,ViewToVM).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.SettingsButtonVisible, view => view.SettingsButton.Visibility).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.BaselineOkImageVisible, view => view.BaselineOkImage.Visibility).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.BatteryImageVisible, view => view.BatteryImage.Visibility).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.SpectrometerConnectedImageVisible, view => view.SpectrometerConnectedImage.Visibility).DisposeWith(disposables);
            });
        }

        private bool ViewToVM(Visibility v)
        {
            if(v == Visibility.Visible)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private Visibility VMToView(bool b)
        {
            if (b)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }
    }
}
