using SpectraCaptureApp.Extension;
using SpectraCaptureApp.Model;
using ReactiveUI;
using Serilog;
using Splat;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Windows;
using SpectraCaptureApp.Infrastructure;

namespace SpectraCaptureApp.ViewModel
{
    public class ScanSubsampleViewModel : ReactiveObject, IRoutableViewModel
    {
        public string UrlPathSegment => "ScanSubsample";

        public ScanCaptureModel Model { get; }
        public IScreen HostScreen { get; }

        public int MaximumScans => 5;

        private int scansCompleted;
        public int ScansCompleted
        {
            get => scansCompleted;
            set => this.RaiseAndSetIfChanged(ref scansCompleted, value);
        }

        readonly ObservableAsPropertyHelper<ScanState> scanState;
        public ScanState ScanStatus => scanState.Value;

        readonly ObservableAsPropertyHelper<bool> scanInProgress;
        public bool ScanInProgress => scanInProgress.Value;

        readonly ObservableAsPropertyHelper<bool> paused;
        public bool Paused => paused.Value;

        public ReactiveCommand<Unit, Unit> PauseCommand { get; }
        public ReactiveCommand<Unit, Unit> ResetCommand { get; }
        public ReactiveCommand<Unit, Unit> StartSubSampleScan { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> SaveCommand { get; }
        

        public ScanSubsampleViewModel(ScanCaptureModel model, IScreen screen = null)
        {
            Model = model;
            HostScreen = screen ?? Locator.Current.GetService<IScreen>();

            PauseCommand = ReactiveCommand.Create(() => 
            {
                Log.Debug("Starting pause");
                Thread.Sleep(TimeSpan.FromSeconds(AppSettings.LoopPauseTime));
                Log.Debug("Paused for {Time}s", AppSettings.LoopPauseTime);
            });

            PauseCommand.IsExecuting.ToProperty(this, vm => vm.Paused, out paused);

            StartSubSampleScan = ReactiveCommand.CreateFromObservable(StartSubSampleScanImpl, this.WhenAnyValue(x => x.ScansCompleted, x => x < MaximumScans));
            StartSubSampleScan.IsExecuting.ToProperty(this, vm => vm.ScanInProgress, out scanInProgress);
            StartSubSampleScan.ThrownExceptions.Subscribe((ex) =>
            {
                ex.HandleWorkflowException(HostScreen, Model, nameof(StartSubSampleScan));
            });
            StartSubSampleScan.Subscribe(x =>
            {
                Log.Debug("Scan completed. Incrementing ScansCompleted {ScansCompletedBefore}->{ScansCompletedAfter}", ScansCompleted, ScansCompleted + 1);
                ScansCompleted += 1;
                if (AppSettings.AutomaticLoop && ScansCompleted < MaximumScans)
                {
                    Log.Debug("Automatic loop is enabled. Executing next subsample scan");
                    StartSubSampleScan.Execute().Subscribe();
                }
            });

            SaveCommand = ReactiveCommand.CreateFromObservable(
                () => 
                {
                    UIServices.SetBusyState();
                    Log.Debug("Starting save process");
                    Model.ScanningWorkflow.TurnOffLamp();
                    Log.Debug("Lamp turned off");
                    Model.ScanningWorkflow.StoreSpectrum();
                    Log.Debug("Spectrum stored");
                    if(AppSettings.AutoReferenceSetting == AutoReferenceSettings.Increment || AppSettings.AutoReferenceSetting == AutoReferenceSettings.DateTime_Increment)
                    {
                        AppSettings.CurrentAutoRefIncrement += 1;
                    }
                    return HostScreen.ResetWorkflow();
                },
                this.WhenAnyValue(
                    x => x.ScansCompleted,
                    x => x.ScanInProgress,                 
                    (scanNumber, isLoading) => 
                    {
                        return scanNumber >= MaximumScans && !ScanInProgress; 
                    })
                );
            SaveCommand.ThrownExceptions.Subscribe((error) =>
            {
                error.HandleWorkflowException(HostScreen, Model, nameof(SaveCommand));
            });

            this.WhenAnyValue(x => x.ScanInProgress).SetBusyCursor();

            this.WhenAnyValue(
                vm => vm.ScanInProgress, 
                vm => vm.Paused, 
                (sip, paused) => 
                {
                    if (paused)
                    {
                        return ScanState.Pause;
                    }
                    if (sip)
                    {
                        return ScanState.Busy;
                    }
                    return ScanState.Ready;
                }).ToProperty(this, vm => vm.ScanStatus, out scanState);

            ResetCommand = ReactiveCommand.Create(ResetImpl);
            ResetCommand.ThrownExceptions.Subscribe((ex) =>
            {
                ex.HandleWorkflowException(HostScreen, Model, nameof(ResetCommand));
            });
        }

        private void ResetImpl()
        {
            ScansCompleted = 0;
        }

        private IObservable<Unit> StartSubSampleScanImpl()
        {
            return Observable.Start(() =>
            {
                PauseCommand.Execute().Subscribe();
                Log.Debug("StartingSubSampleScan {ScanNumber}/{MaxiumScans}", ScansCompleted + 1, MaximumScans);
                CaptureSubSampleScan();
                Log.Debug("Taken subsamplescan successfully");

            }).ObserveOn(RxApp.MainThreadScheduler)
            .TakeUntil(HostScreen.Router.NavigateAndReset);
        }

        private int failedAttempts = 0;
        private void CaptureSubSampleScan()
        {
            var result = Model.ScanningWorkflow.ScanSubSample();
            if (result.IsValid)
            {
                Log.Debug("Successfully scanned subsample. Scan number = {ScanNumber}/{MaximumScansScan}", this.ScansCompleted + 1, MaximumScans);
            }
            else
            {
                throw new Exception($"Subsample scan {ScansCompleted + 1}/{MaximumScans} was invalid.");
                //failedAttempts++;
                //Log.Warning("Invalid subsample scan.");
                //if (failedAttempts < AppSettings.RetryAttempts)
                //{
                //    //Navtigate to error view
                //}
            }
        }
    }   
}
