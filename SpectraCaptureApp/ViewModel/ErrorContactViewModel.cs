using ReactiveUI;
using SpectraCaptureApp.Model;
using Splat;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpectraCaptureApp.ViewModel
{
    public class ErrorContactViewModel : ReactiveObject, IRoutableViewModel
    {
        public string UrlPathSegment => "Error";

        public ScanCaptureModel Model { get; }
        public IScreen HostScreen { get; }

        public ErrorContactViewModel(ScanCaptureModel model, IScreen screen = null)
        {
            Model = model;
            HostScreen = screen ?? Locator.Current.GetService<IScreen>();
        }
    }
}
