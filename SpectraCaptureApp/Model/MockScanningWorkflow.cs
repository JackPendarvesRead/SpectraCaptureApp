using Aunir.SpectrumAnalysis2.Interfaces;
using NIR4.ViaviCapture.Model;
using Serilog;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace SpectraCaptureApp.Model
{
    public class MockScanningWorkflow : IScanningWorkflow
    {
        private readonly string saveDirectory;
        private int pauseTime = 400;

        public MockScanningWorkflow(string saveDirectory)
        {
            this.saveDirectory = saveDirectory;
        }

        public ValidationResult ScanReference()
        {
            Log.Debug($"Scanning reference");
            Thread.Sleep(pauseTime);            
            return ValidationResult.Valid();
        }

        public ValidationResult ScanSubSample()
        {
            Log.Debug($"Scanning subsample");
            Thread.Sleep(pauseTime);
            return ValidationResult.Valid();
        }

        public void SetSampleReference(string sampleReference)
        {
            Log.Debug($"Set sample reference to {sampleReference}");
            Thread.Sleep(pauseTime);
        }

        public void StoreSpectrum()
        {
            Log.Debug($"File saved saved to: {saveDirectory}");
            Thread.Sleep(pauseTime);
            MessageBox.Show($"File saved saved to: {saveDirectory}");
        }

        public void TurnOffLamp()
        {
            Log.Debug("Turning off lamp");
            Thread.Sleep(pauseTime);
        }
        public void TurnOnLamp()
        {
            Log.Debug("Turning on lamp");
            Thread.Sleep(pauseTime);
        }

        //protected override ValidationResult ValidateDarkReferenceScan(List<float> darkReferenceScan)
        //{
        //    if(darkReferenceScan.Where(x => x > 100).Any())
        //    {
        //        return new ValidationResult.;
        //    }
        //    return true;
        //}

        //protected override ValidationResult ValidateCumulativeSubSampeScans(
        //    List<float> darkScan, 
        //    List<float> lightScan, 
        //    List<ISpectrumData> previousSubSampleScans, 
        //    ISpectrumData newSubSampleScan)
        //{
        //    // Do the PCA validation

        //    return new ValidationResult();
        //}
    }
}
