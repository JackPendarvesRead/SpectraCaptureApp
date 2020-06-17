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

namespace SpectraCaptureApp.View
{
    public partial class ScanSubsampleView : ReactiveUserControl<ScanSubsampleViewModel>
    {
        public ScanSubsampleView()
        {
            InitializeComponent();
            this.WhenActivated(disposables =>
            {
                //this.OneWayBind(ViewModel, vm => vm.UrlPathSegment, view => view..Text).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.ScansCompleted, view => view.ScansCompleted.Text).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.ScanInProgress, view => view.IsLoading.Text, ScanInProgressToStringVmToView).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.Paused, view => view.Paused.Text).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.ScanStatus, view => view.Status.Text, x => x.ToString()).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.StartSubSampleScan, view => view.ScanButton).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.SaveCommand, view => view.SaveButton).DisposeWith(disposables);
            });
        }

        private string ScanInProgressToStringVmToView(bool scanInProgress)
        {
            if (scanInProgress)
            {
                return "Scan In Progress";
            }
            else
            {
                return "Ready";
            }
        }
    }
}
