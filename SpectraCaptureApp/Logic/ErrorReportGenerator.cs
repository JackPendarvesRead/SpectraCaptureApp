using Aunir.SpectrumAnalysis2.Interfaces;
using SpectraCaptureApp.Model;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;

namespace SpectraCaptureApp.Logic
{
    public static class ErrorReportGenerator
    {
        public static ErrorReport Generate(ScanCaptureModel model, string sampleType, string comments)
        {
            var report = new ErrorReport
            {
                Comments = "",
                DateGenerated = DateTime.UtcNow,
                ExceptionsThrown = null,
                InstrumentModel = "",
                InstrumentSerial = "",
                SampleReference = "",
                SampleType = "",
                User = "",
                DarkScan = new List<float>(),
                WhiteScan = new List<float>(),
                Spectra = new List<ISpectrumData>()
            };
            return report;
        }

        public static ErrorReport GenerateDummy()
        {
            return new ErrorReport
            {
                Comments = "This is a comment. The sample was very dark and dry.",
                DateGenerated = DateTime.UtcNow,
                ExceptionsThrown = null,
                InstrumentModel = "JDSU",
                InstrumentSerial = "A1-00100",
                SampleReference = "corn123456",
                SampleType = "Maize (Corn)",
                User = "JackPRead",
                DarkScan = new List<float> { 1, 2, 3, 4 },
                WhiteScan = new List<float> { 500, 550 , 600, 660 },
                Spectra = null
            };
        }

        private static List<Exception> GetExceptionsList(IObservable<Exception> exceptions)
        {
            var list = new List<Exception>();
            return null;
        }
    }
}
