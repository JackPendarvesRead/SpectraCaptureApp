using SpectraCaptureApp.Model;
using SpectraCaptureApp.View;
using ReactiveUI;
using Serilog;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SpectraCaptureApp.ViewModel.Controls;
using System.Reactive.Linq;

namespace SpectraCaptureApp.ViewModel
{
    public class MainWindowViewModel : ReactiveObject, IScreen
    {
        public TopBarViewModel TopBarViewModel { get; set; }
        public RoutingState Router { get; }

        private bool throwErrors;
        public bool ThrowErrors
        {
            get => throwErrors;
            set => this.RaiseAndSetIfChanged(ref throwErrors, value);
        }

        public MainWindowViewModel(RoutingState testRouter = null)
        {
            Router = testRouter ?? new RoutingState();
            TopBarViewModel = new TopBarViewModel(this);

            TopBarViewModel.SpectrometerIsConnected = true;

            TopBarViewModel.AbortCommand.Execute();

            this.WhenAnyValue(vm => vm.ThrowErrors).Subscribe((throwError) => TestSettings.ThrowErrors = throwError);
            this.WhenAnyValue(vm => vm.TopBarViewModel.BaselineIsOk).Subscribe((baselineOk) => TestSettings.BaselineOk = baselineOk);
            this.WhenAnyValue(vm => vm.TopBarViewModel.SpectrometerIsConnected).Subscribe(
                (specConnected) =>
                {
                    TestSettings.SpectrometerConnected = specConnected;
                    if (!specConnected)
                    {
                        Log.Warning("Spectrometer disconnected event");
                        MessageBox.Show(
                            "Spectrometer was disconnected. Please reconnect before continuing scanning.",
                            "Spectrometer Disconnected",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                    }
                });
        }
    }
}
