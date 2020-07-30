using Aunir.InstrumentControl.Interfaces;
using Aunir.InstrumentControl.Jdsu;
using NIR4.ViaviCapture.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpectraCaptureApp.Model
{
    public class MyNirDeviceConnectionFactory : INirDeviceConnectionFactory
    {
        private readonly JdsuDeviceController controller;

        public MyNirDeviceConnectionFactory()
        {
            controller = new JdsuDeviceController();
        }

        public INirDeviceConnection GetConnection()
        {
            var devices = controller.GetConnectedDevices();

            if (devices.Length < 1)
                throw new HardwareException("No Devices Found");

            if (devices.Length > 1)
                throw new HardwareException("More than one device found");

            return controller.CreateConnection(devices[0]);
        }
    }
}
