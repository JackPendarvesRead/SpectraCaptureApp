using ReactiveUI;
using SpectraCaptureApp.ViewModel;
using System.Reactive.Disposables;

namespace SpectraCaptureApp.View
{
    public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new MainWindowViewModel();
            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, vm => vm.Router, view => view.RoutedViewHost.Router).DisposeWith(disposables);

                this.Bind(ViewModel, vm => vm.TopBarViewModel, view => view.TopBar.ViewModel).DisposeWith(disposables);

                this.Bind(ViewModel, vm => vm.TopBarViewModel.SpectrometerIsConnected, view => view.SpecConnectedCheck.IsChecked).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.TopBarViewModel.BaselineIsOk, view => view.BaselineCheck.IsChecked).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.ThrowErrors, view => view.ThrowErrorCheck.IsChecked).DisposeWith(disposables);
            });
        }
    }
}
