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
    public class EnterSampleReferenceViewModel : ReactiveObject, IRoutableViewModel
    {
        public string UrlPathSegment => "SampleReference";

        public ScanCaptureModel Model { get; }
        public IScreen HostScreen { get; }        

        public ReactiveCommand<Unit, IRoutableViewModel> SetSampleReferenceCommand { get; }
        public ReactiveCommand<Unit, Unit> AutoReferenceCommand { get; }

        public List<string> CalibrationSets { get; set; } = new List<string> { "Corn", "Wheat", "Pig" };

        public EnterSampleReferenceViewModel(ScanCaptureModel model, IScreen screen = null)
        {
            Model = model;
            HostScreen = screen ?? Locator.Current.GetService<IScreen>();

            AutoReferenceCommand = ReactiveCommand.Create(() =>
            {
                Model.SampleReference = DateTime.UtcNow.ToString();
            });

            SetSampleReferenceCommand = ReactiveCommand.CreateFromObservable(() => 
            {
                UIServices.SetBusyState();
                Model.ScanningWorkflow.SetSampleReference(Model.SampleReference);
                Log.Debug("Sample reference set to: {SampleReference}", Model.SampleReference);
                return HostScreen.Router.Navigate.Execute(new ScanReferenceViewModel(Model, HostScreen));
            },
            this.WhenAnyValue(x => x.Model.SampleReference, sr => !string.IsNullOrWhiteSpace(sr)));
            SetSampleReferenceCommand.ThrownExceptions.Subscribe((error) =>
            {
                MessageBox.Show(error.Message);
            });
        }
    }
}
