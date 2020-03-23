using ReactiveUI;
using SpectraCaptureApp.ViewModel;
using System.Reactive.Disposables;

namespace SpectraCaptureApp
{
    public partial class MainWindow : ReactiveWindow<MainViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new MainViewModel();
            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, vm => vm.Router, view => view.RoutedViewHost.Router).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.Home, view => view.HomeButton).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.NewScan, view => view.RestartButton).DisposeWith(disposables);
            });
        }
    }
}
