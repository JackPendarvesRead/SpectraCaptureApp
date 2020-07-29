using SpectraCaptureApp.Model;
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
using SpectraCaptureApp.Extension;

namespace SpectraCaptureApp.ViewModel
{
    public class ScanReferenceViewModel : ReactiveObject, IRoutableViewModel
    {

        public string UrlPathSegment => "ScanReference";

        public ScanCaptureModel Model { get; }
        public IScreen HostScreen { get; }

        public ReactiveCommand<Unit, Unit> ScanReferenceCommand { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> SubSambleScanNavigateCommand { get; }

        public ScanReferenceViewModel(ScanCaptureModel model, IScreen screen = null)
        {
            Model = model;
            HostScreen = screen ?? Locator.Current.GetService<IScreen>();

            ScanReferenceCommand = ReactiveCommand.Create(() =>
            {
                UIServices.SetBusyState();
                var result = Model.ScanningWorkflow.ScanReference();
                if (result.IsValid)
                {
                    Log.Debug("Reference scan taken successfully");
                    SubSambleScanNavigateCommand.Execute();
                }
                else
                {
                    var ignoreWarning = MessageBox.Show("Baseline scan was invalid. Continue?", "Baseline was invalid", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (ignoreWarning == MessageBoxResult.Yes)
                    {
                        SubSambleScanNavigateCommand.Execute();
                    }
                }

            });
            ScanReferenceCommand.ThrownExceptions.Subscribe((error) =>
            {
                error.HandleWorkflowException(HostScreen, Model, nameof(ScanReferenceCommand));
            });

            SubSambleScanNavigateCommand = ReactiveCommand.CreateFromObservable(() =>
            {
                return HostScreen.Router.Navigate.Execute(new ScanSubsampleViewModel(Model, HostScreen));
            });
            SubSambleScanNavigateCommand.ThrownExceptions.Subscribe((error) =>
            {
                error.HandleWorkflowException(HostScreen, Model, nameof(SubSambleScanNavigateCommand));
            });
        }
    }
}
