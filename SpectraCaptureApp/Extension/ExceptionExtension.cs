using ReactiveUI;
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
        public static void HandleWorkflowException(this Exception ex, IScreen hostScreen, ScanCaptureModel model)
        {
            model.WorkflowExceptions.Add(ex);
            if(model.WorkflowExceptions.Count >= AppSettings.RetryAttempts)
            {
                hostScreen.Router.Navigate.Execute(new ErrorContactViewModel(model, hostScreen));
            }
            else
            {
                MessageBox.Show(ex.Message, "Error occurred in Scanning Workflow", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
