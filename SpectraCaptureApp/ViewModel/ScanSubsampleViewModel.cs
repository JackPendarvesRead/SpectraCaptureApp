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

        private ScanState scanState;
        public ScanState ScanState
        {
            get => scanState;
            set => this.RaiseAndSetIfChanged(ref scanState, value);
        }

        private string scanStateString;
        public string ScanStateString
        {
            get => scanStateString;
            set => this.RaiseAndSetIfChanged(ref scanStateString, value);
        }

        readonly ObservableAsPropertyHelper<bool> scanInProgress;
        public bool ScanInProgress => scanInProgress.Value;

        public ReactiveCommand<Unit, Unit> StartSubSampleScan { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> Save { get; }

        public ScanSubsampleViewModel(ScanCaptureModel model, IScreen screen = null)
        {
            Model = model;
            HostScreen = screen ?? Locator.Current.GetService<IScreen>();
            scanState = ScanState.Ready;

            StartSubSampleScan = ReactiveCommand.CreateFromObservable(StartSubSampleScanImpl, this.WhenAnyValue(x => x.ScansCompleted, x => x < MaximumScans));            
            StartSubSampleScan.IsExecuting.ToProperty(this, x => x.ScanInProgress, out scanInProgress);
            StartSubSampleScan.Subscribe(x =>
            {
                ScansCompleted += 1;
                scanState = ScanState.Ready;
                if(AppSettings.AutomaticLoop && ScansCompleted < MaximumScans)
                {
                    StartSubSampleScan.Execute().Subscribe();
                }
            });

            this.WhenAnyValue(vm => vm.ScanState)
                .ObserveOn(Scheduler.CurrentThread)
                .SubscribeOn(RxApp.MainThreadScheduler)
                .Subscribe(state => 
                {
                    ScanStateString = state.ToString(); 
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
                    x => x.ScansCompleted,
                    x => x.ScanInProgress,                 
                    (scanNumber, isLoading) => 
                    {
                        return scanNumber >= MaximumScans && !ScanInProgress; 
                    })
                );
            Save.ThrownExceptions.Subscribe((error) =>
            {
                Log.Error(error, "Save method failed");
                MessageBox.Show(error.Message,
                   "Save Failed",
                   MessageBoxButton.OK,
                   MessageBoxImage.Error);
                //Navigate to error view?
            });

            this.WhenAnyValue(x => x.ScanInProgress).SetBusyCursor();
        }

        private int failedAttempts = 0;
        private IObservable<Unit> StartSubSampleScanImpl()
        {
            return Observable.Start(() =>
            {
                Dispatcher.CurrentDispatcher.Invoke(() => { this.scanState = ScanState.Pause; });
                this.scanState = ScanState.Pause;
                Thread.Sleep(TimeSpan.FromSeconds(AppSettings.LoopPauseTime));
                this.scanState = ScanState.Busy;
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
            });            
        }
    }
}
