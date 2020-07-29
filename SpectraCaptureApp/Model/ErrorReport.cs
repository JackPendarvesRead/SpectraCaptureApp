using Aunir.SpectrumAnalysis2.Interfaces;
using NIR4.ViaviCapture.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpectraCaptureApp.Model
{
    public class ErrorReport
    {
        public string User { get; set; }
        public DateTime DateGenerated { get; set; }
        public string SampleType { get; set; }
        public string Comments { get; set; }        
        public List<string> ExceptionsThrownInformation { get; set; }
        public ScanContext ScanContext { get; set; }
    }
}
