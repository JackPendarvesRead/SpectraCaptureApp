﻿using Aunir.SpectrumAnalysis2.Interfaces;
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
        private bool ThrowErrors => TestSettings.ThrowErrors;
        private readonly int pauseTime = 1000;

        public MockScanningWorkflow(string saveDirectory)
        {
            this.saveDirectory = saveDirectory;
        }

        public ValidationResult ScanReference()
        {
            if (ThrowErrors)
                throw new Exception("ScanReference method failed");

            Log.Debug($"Scanning reference");
            Thread.Sleep(pauseTime);

            if (TestSettings.BaselineOk)
            {
                return ValidationResult.Valid();
            }
            return ValidationResult.NotValid();
        }

        public ValidationResult ScanSubSample()
        {
            if (ThrowErrors)
                throw new Exception("ScanSubSample method failed"); 
            
            Log.Debug($"Scanning subsample");
            Thread.Sleep(pauseTime);
            return ValidationResult.Valid();
        }

        public void SetSampleReference(string sampleReference)
        {
            if (ThrowErrors)
                throw new Exception("SetSampleReference method failed"); 
           
            Log.Debug($"Set sample reference to {sampleReference}");
            Thread.Sleep(pauseTime);
        }

        public void StoreSpectrum()
        {
            if (ThrowErrors)
                throw new Exception("StoreSpectrum method failed");
            
            Log.Debug($"File saved saved to: {saveDirectory}");

            Thread.Sleep(pauseTime);
            MessageBox.Show($"File saved saved to: {saveDirectory}");
        }

        public void TurnOffLamp()
        {
            if (ThrowErrors)
                throw new Exception("TurnOffLamp method failed"); 
            
            Log.Debug("Turning off lamp");
            Thread.Sleep(pauseTime);
        }
        public void TurnOnLamp()
        {
            if (ThrowErrors)
                throw new Exception("TurnOnLamp method failed"); 
            
            
            Log.Debug("Turning on lamp");
            Thread.Sleep(pauseTime);
        }
    }
}
