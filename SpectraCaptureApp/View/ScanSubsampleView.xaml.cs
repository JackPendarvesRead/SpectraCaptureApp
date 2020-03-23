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
                this.OneWayBind(ViewModel, vm => vm.UrlPathSegment, view => view.PathTextBlock.Text).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.Model.ScanNumber, view => view.ScanNumber.Text).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.ScanInProgress, view => view.IsLoading.Text);
                this.BindCommand(ViewModel, vm => vm.CaptureScan, view => view.ScanButton).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.Save, view => view.SaveButton).DisposeWith(disposables);
            });
        }
    }
}
