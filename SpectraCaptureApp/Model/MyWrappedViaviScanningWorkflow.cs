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

        public MyWrappedViaviScanningWorkflow() 
            : base(new MicroNirUsbDeviceConnectionFactory(), "")
        {
        }

        public MyWrappedViaviScanningWorkflow(INirDeviceConnectionFactory deviceConnectionFactory, string storageDirectory, int numberOfScansPerSubsample = 50) 
            : base(deviceConnectionFactory, storageDirectory, numberOfScansPerSubsample)
        {     
        }
    }
}
