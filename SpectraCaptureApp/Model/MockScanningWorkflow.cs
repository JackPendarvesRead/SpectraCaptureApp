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
        private int pauseTime = 400;

        public MockScanningWorkflow()
        {            
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
            MessageBox.Show("File saved!!!");
        }

        public void TurnOffLamp()
        {
            Thread.Sleep(pauseTime);
        }
        public void TurnOnLamp()
        {
            Thread.Sleep(pauseTime);
        }
    }
}
