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
            });
        }
    }
}
