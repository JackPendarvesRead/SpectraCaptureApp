using Aunir.SpectrumAnalysis2.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpectraCaptureApp.Model
{
    public class ErrorReport
    {
        public string User { get; set; }
        public DateTime DateGenerated { get; set; }
        public string SampleReference { get; set; }
        public string SampleType { get; set; }
        public string Comments { get; set; }
        public string InstrumentModel { get; set; }
        public string InstrumentSerial { get; set; }
        public List<float> WhiteScan { get; set; }
        public List<float> DarkScan { get; set; }
        public List<ISpectrumData> Spectra { get; set; }
        public List<Exception> ExceptionsThrown { get; set; }
    }
}
