using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectraCaptureApp.Model
{
    public interface IScanningWorkflow
    {
        void ScanReference();
        void ScanSubSample();
        void SetSampleReference(string sampleReference);
        void StoreSpectrum();
        void TurnOffLamp();
    }
}
