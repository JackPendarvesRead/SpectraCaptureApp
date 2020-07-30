using Aunir.InstrumentControl.Interfaces;
using ReactiveUI;
using Serilog;
using SpectraCaptureApp.Model;
using SpectraCaptureApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace SpectraCaptureApp.Extension
{
    public static class ExceptionExtension
    {
        public static void HandleWorkflowException(this Exception ex, IScreen hostScreen, ScanCaptureModel model, string methodName)
        {
            if(ex is HardwareException hwex)
            {
                Log.Error(hwex, hwex.Message);
                MessageBox.Show("There was a problem connecting to the spectrometer. Please ensure that spectrometer is connected and restart scan process.", "Hardware Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                hostScreen.ResetWorkflow();
            }
            else
            {
                string message = $"{methodName} method failed";
                Log.Error(ex, message);
                model.WorkflowExceptions.Add(ex);
                if (model.WorkflowExceptions.Count >= AppSettings.RetryAttempts)
                {
                    hostScreen.Router.Navigate.Execute(new ErrorContactViewModel(model, hostScreen));
                }
                else
                {
                    MessageBox.Show(ex.Message, message, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }           
           
        }
    }
}
