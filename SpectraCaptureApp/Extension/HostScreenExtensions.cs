using ReactiveUI;
using SpectraCaptureApp.Model;
using SpectraCaptureApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpectraCaptureApp.Extension
{
    public static class HostScreenExtensions
    {
        public static IObservable<IRoutableViewModel> ResetWorkflow(this IScreen hostScreen)
        {
            return hostScreen.Router.NavigateAndReset.Execute(new EnterSampleReferenceViewModel(new ScanCaptureModel(), hostScreen));
        }
    }
}
