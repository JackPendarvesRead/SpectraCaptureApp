﻿using System;
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

        public void ScanReference()
        {
            Thread.Sleep(pauseTime);
        }

        public void ScanSubSample()
        {
            Thread.Sleep(pauseTime);
        }

        public void SetSampleReference(string sampleReference)
        {
            Thread.Sleep(pauseTime);
        }

        public void StoreSpectrum()
        {
            Thread.Sleep(pauseTime);
            MessageBox.Show("File saved to " + AppSettings.SpectrumSaveDirectory);
        }

        public void TurnOffLamp()
        {
            Thread.Sleep(pauseTime);
        }
    }
}