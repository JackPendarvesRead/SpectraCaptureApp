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

        public ReactiveCommand<Unit, IRoutableViewModel> ScanReferenceCommand { get; }

        public ScanReferenceViewModel(ScanCaptureModel model, IScreen screen = null)
        {
            Model = model;
            HostScreen = screen ?? Locator.Current.GetService<IScreen>();

            ScanReferenceCommand = ReactiveCommand.CreateFromObservable(() =>
            {
                UIServices.SetBusyState();
                var result = Model.ScanningWorkflow.ScanReference();
                if (result.IsValid)
                {
                    Log.Debug("Reference scan taken successfully");
                    return HostScreen.Router.Navigate.Execute(new ScanSubsampleViewModel(Model, HostScreen));
                }
                else
                {
                    var ignoreWarning = MessageBox.Show("Baseline scan was invalid. Continue?", "Baseline was invalid", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if(ignoreWarning == MessageBoxResult.Yes)
                    {
                        return HostScreen.Router.Navigate.Execute(new ScanSubsampleViewModel(Model, HostScreen));
                    }
                    return null;
                }
                
            });
            ScanReferenceCommand.ThrownExceptions.Subscribe((error) =>
            {
                Log.Error(error, "ScanReferenceCommand Failed");
                error.HandleWorkflowException(HostScreen, Model);
            });
        }
    }
}
