using SpectraCaptureApp.Extension;
using SpectraCaptureApp.Model;
using ReactiveUI;
using Serilog;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Aunir.SpectrumAnalysis2.Interfaces;
using NIR4.ViaviCapture.Model;

namespace SpectraCaptureApp.ViewModel
{
    public class ScanSubsampleViewModel : ReactiveObject, IRoutableViewModel
    {
        public string UrlPathSegment => "ScanSubsample";

        public ScanCaptureModel Model { get; }
        public IScreen HostScreen { get; }


        readonly ObservableAsPropertyHelper<bool> _scanInProgress;
        public bool ScanInProgress
        {
            get => _scanInProgress.Value;
        }

        public ReactiveCommand<Unit, Unit> CaptureScan { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> Save { get; }
        
        public ScanSubsampleViewModel(ScanCaptureModel model, IScreen screen = null)
        {
            Model = model;
            HostScreen = screen ?? Locator.Current.GetService<IScreen>();

            if(Model.ScanningWorkflow is MyWrappedViaviScanningWorkflow)
            {
                var spectraField = typeof(NIR4.ViaviCapture.Model.ViaviScanningWorkflow).GetField("spectra", BindingFlags.NonPublic | BindingFlags.Instance);
                var spectra = (List<ISpectrumData>)spectraField.GetValue(Model.ScanningWorkflow);
            }

            CaptureScan = ReactiveCommand.CreateFromObservable(CaptureScanImpl, 
                this.WhenAnyValue(x => x.Model.ScanNumber, x => x < 5));
            CaptureScan.IsExecuting.ToProperty(this, x => x.ScanInProgress, out _scanInProgress);
            this.WhenAnyValue(x => x.ScanInProgress).SetBusyCursor();
            CaptureScan.ThrownExceptions.Subscribe(
                (error) =>
                {
                    MessageBox.Show(error.Message,
                    "Scan Capture Method Failed",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                    Save.Execute();
                });
            CaptureScan.IsExecuting
               .Skip(1)
               .Where(isExecuting => !isExecuting)
               .Subscribe(x =>
               {
                   Model.ScanNumber++;
               });

            Save = ReactiveCommand.CreateFromObservable(
                () => 
                {
                    UIServices.SetBusyState();
                    Model.ScanningWorkflow.TurnOffLamp();
                    Log.Debug("Lamp turned off");
                    Model.ScanningWorkflow.StoreSpectrum();
                    Log.Debug("Spectrum stored");
                    return HostScreen.Router.NavigateAndReset.Execute(new EnterSampleReferenceViewModel(new ScanCaptureModel(), HostScreen));
                },
                this.WhenAnyValue(
                    x => x.Model.ScanNumber,
                    x => x.ScanInProgress,                 
                    (scanNumber, isLoading) => 
                    {
                        return scanNumber >= model.MinimumScanCount && !ScanInProgress; 
                    })
                );
            Save.ThrownExceptions.Subscribe((error) =>
            {
                Log.Error(error, "Save method failed");
                MessageBox.Show(error.Message,
                   "Save Failed",
                   MessageBoxButton.OK,
                   MessageBoxImage.Error);
            });
        }

        private int failedAttempts = 0;
        public IObservable<Unit> CaptureScanImpl()
        {
            return Observable.Start(() =>
            {
                var result = Model.ScanningWorkflow.ScanSubSample();
                if (result.IsValid)
                {
                    Log.Debug("Successfully scanned subsample. Scan number = {ScanNumber}", this.Model.ScanNumber);
                    
                }
                else
                {
                    failedAttempts++;
                    Log.Warning("Invalid subsample scan.");
                    if (failedAttempts < AppSettings.RetryAttempts)
                    {

                    }
                }
            });
        }
    }
}
