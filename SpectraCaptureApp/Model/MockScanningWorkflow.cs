using Aunir.SpectrumAnalysis2.Interfaces;
using NIR4.ViaviCapture.Model;
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
            Thread.Sleep(pauseTime);            
            return ValidationResult.Valid();
        }

        public ValidationResult ScanSubSample()
        {
            Thread.Sleep(pauseTime);
            return ValidationResult.Valid();
        }

        public void SetSampleReference(string sampleReference)
        {
            Thread.Sleep(pauseTime);
        }

        public void StoreSpectrum()
        {
            Thread.Sleep(pauseTime);
            MessageBox.Show($"File saved saved to: {saveDirectory}");
        }

        public void TurnOffLamp()
        {
            Thread.Sleep(pauseTime);
        }
        public void TurnOnLamp()
        {
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
