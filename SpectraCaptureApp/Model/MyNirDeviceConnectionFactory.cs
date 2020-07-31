﻿using Aunir.InstrumentControl.Interfaces;
using Aunir.InstrumentControl.Jdsu;
using NIR4.ViaviCapture.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpectraCaptureApp.Model
{
    public class MyNirDeviceConnectionFactory : INirDeviceConnectionFactory
    {
        private readonly JdsuDeviceController controller = new JdsuDeviceController();
        private static INirDeviceConnection connection;
        private static string currentFtHandle;

        public INirDeviceConnection GetConnection()
        {
            if(connection == null)
            {
                SetConnection();
            }
            else
            {
                var connected = CheckDeviceIsConnected();
                if (!connected)
                {
                    connection = null;
                    SetConnection();
                }
            }
            return connection;
        }

        private bool CheckDeviceIsConnected()
        {
            var ftHandle = GetFtHandle();
            return ftHandle == currentFtHandle;
        }

        private void SetConnection()
        {
            var devices = controller.GetConnectedDevices();
            if (devices.Length < 1)
                throw new HardwareException("No Devices Found");

            if (devices.Length > 1)
                throw new HardwareException("More than one device found");

            connection = controller.CreateConnection(devices[0]);
            currentFtHandle = GetFtHandle();
        }

        private string GetFtHandle()
        {
            return controller.GetCurrentFtHandleString();
        }
    }
}
