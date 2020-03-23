using SpectraCaptureApp.Model;
using SpectraCaptureApp.View;
using ReactiveUI;
using Serilog;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace SpectraCaptureApp.ViewModel
{
    public class MainViewModel : ReactiveObject, IScreen
    {
        public RoutingState Router { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> Home { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> NewScan { get; }

        public MainViewModel()
        {
            Router = new RoutingState();

            Home = ReactiveCommand.CreateFromObservable(() 
                => Router.NavigateAndReset.Execute(new HomeViewModel(this)));
            NewScan = ReactiveCommand.CreateFromObservable(() 
                => Router.NavigateAndReset.Execute(new EnterSampleReferenceViewModel(new ScanCaptureModel(), this)));

            NewScan.Execute();
        }
    }
}
