using Aunir.SpectrumAnalysis2.Interfaces;
using NIR4.ViaviCapture.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectraCaptureApp.Model
{
    public class MyWrappedViaviScanningWorkflow : ViaviScanningWorkflow
    {
        public List<ISpectrumData> ValidSpectra { get; set; } = new List<ISpectrumData>();
        public List<ISpectrumData> InvalidSpectra { get; set; } = new List<ISpectrumData>();

        public MyWrappedViaviScanningWorkflow(string storageDirectory) 
            : base(new MicroNirUsbDeviceConnectionFactory(), "storageDirectory")
        {
        }

        public MyWrappedViaviScanningWorkflow(INirDeviceConnectionFactory deviceConnectionFactory, string storageDirectory, int numberOfScansPerSubsample = 50) 
            : base(deviceConnectionFactory, storageDirectory, numberOfScansPerSubsample)
        {     
        }

        protected override ValidationResult ValidateDarkReferenceScan(List<float> darkReferenceScan)
        {
            if (darkReferenceScan.Where(x => x > TestSettings.MaximumDarkCount).Any())
            {
                return ValidationResult.NotValid();
            }
            return ValidationResult.Valid();
        }

        protected override ValidationResult ValidateLightReferenceScan(List<float> lightReferenceScan)
        {
            if (lightReferenceScan.Where(x => x < TestSettings.MinimumLightCount).Any())
            {
                return ValidationResult.NotValid();
            }
            return ValidationResult.Valid();
        }

        protected override ValidationResult ValidateCumulativeSubSampeScans(
            List<float> darkScan, 
            List<float> lightScan,
            List<ISpectrumData> previousSubSampleScans, 
            ISpectrumData newSubSampleScan)
        {
            if (ValidateSpectrumData(newSubSampleScan))
            {
                ValidSpectra.Add(newSubSampleScan);
                return ValidationResult.Valid();
            }
            else
            {
                InvalidSpectra.Add(newSubSampleScan);
                return ValidationResult.NotValid();
            }
        }

        private bool ValidateSpectrumData(ISpectrumData data)
        {
            return true;
        }
    }
}
