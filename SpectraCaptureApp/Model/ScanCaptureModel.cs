using NIR4.ViaviCapture.Model;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectraCaptureApp.Model
{
    public class ScanCaptureModel : ReactiveObject
    {
        public ScanCaptureModel(IScanningWorkflow scanningWorkflow = null)
        {
            this.ScanningWorkflow = scanningWorkflow ?? Locator.Current.GetService<IScanningWorkflow>();
        }

        public IScanningWorkflow ScanningWorkflow { get; }

        private string sampleReference;
        public string SampleReference
        {
            get => sampleReference;
            set => this.RaiseAndSetIfChanged(ref sampleReference, value);
        }

        public List<Exception> WorkflowExceptions = new List<Exception>();
    }
}
