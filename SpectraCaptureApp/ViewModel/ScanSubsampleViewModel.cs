using SpectraCaptureApp.Extension;
using SpectraCaptureApp.Model;
using ReactiveUI;
using Serilog;
using Splat;
using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Windows;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;

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

        //private ScanState scanState;
        //public ScanState ScanState
        //{
        //    get => scanState;
        //    set => this.RaiseAndSetIfChanged(ref scanState, value);
        //}
        ////public ScanState ScanStatus
        //{
        //    get
        //    {
        //        if (Paused)
        //        {
        //            return ScanState.Pause;
        //        }
        //        if (ScanInProgress)
        //        {
        //            return ScanState.Busy;
        //        }
        //        return ScanState.Ready;
        //    }
        //}

        readonly ObservableAsPropertyHelper<ScanState> scanState;
        public ScanState ScanStatus => scanState.Value;

        readonly ObservableAsPropertyHelper<bool> scanInProgress;
        public bool ScanInProgress => scanInProgress.Value;

        readonly ObservableAsPropertyHelper<bool> paused;
        public bool Paused => paused.Value;

        public ReactiveCommand<Unit, Unit> PauseCommand { get; }
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
                Log.Error(ex, "Exception thrown in StartSubSampleScan: " + ex.Message);
                Model.WorkflowExceptions.Add(ex);
                if(Model.WorkflowExceptions.Count >= AppSettings.RetryAttempts)
                {                    
                    HostScreen.Router.Navigate.Execute(new ErrorContactViewModel(Model, HostScreen));
                }
                else
                {
                    MessageBox.Show($"Error occurred in Scanning Subsample - {ex.Message}");
                }

            });
            StartSubSampleScan.Subscribe(x =>
            {
                ScansCompleted += 1;
                if (AppSettings.AutomaticLoop && ScansCompleted < MaximumScans)
                {
                    StartSubSampleScan.Execute().Subscribe();
                }
            });

            SaveCommand = ReactiveCommand.CreateFromObservable(
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
                    x => x.ScansCompleted,
                    x => x.ScanInProgress,                 
                    (scanNumber, isLoading) => 
                    {
                        return scanNumber >= MaximumScans && !ScanInProgress; 
                    })
                );
            SaveCommand.ThrownExceptions.Subscribe((error) =>
            {
                Log.Error(error, "Save method failed");
                MessageBox.Show(error.Message,
                   "Save Failed",
                   MessageBoxButton.OK,
                   MessageBoxImage.Error);
                //Navigate to error view?
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
        }

        private IObservable<Unit> StartSubSampleScanImpl()
        {
            return Observable.Start(() =>
            {
                PauseCommand.Execute().Subscribe();
                Log.Debug("StartingSubSampleScan");
                CaptureSubSampleScan();
                Log.Debug("Taken subsamplescan");

            }).ObserveOn(RxApp.MainThreadScheduler)
            .TakeUntil(HostScreen.Router.NavigateAndReset);
        }

        private int failedAttempts = 0;
        private void CaptureSubSampleScan()
        {
            if (Model.ScanningWorkflow.ScanSubSample().IsValid)
            {
                Log.Debug("Successfully scanned subsample. Scan number = {ScanNumber}/{MaximumScansScan}", this.ScansCompleted + 1, MaximumScans);
            }
            else
            {
                failedAttempts++;
                Log.Warning("Invalid subsample scan.");
                if (failedAttempts < AppSettings.RetryAttempts)
                {
                    //Navtigate to error view
                }
            }
        }
    }   
}
