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
using System.Windows;
using SpectraCaptureApp.ViewModel.Controls;
using System.Reactive.Linq;

namespace SpectraCaptureApp.ViewModel
{
    public class MainWindowViewModel : ReactiveObject, IScreen
    {
        public TopBarViewModel TopBarViewModel { get; set; }
        public RoutingState Router { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> SettingsNavigateCommand { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> NewScan { get; }

        IRoutableViewModel CurrentViewModel { get; }

        public MainWindowViewModel(RoutingState testRouter = null)
        {
            Router = testRouter ?? new RoutingState();
            TopBarViewModel = new TopBarViewModel();

            SettingsNavigateCommand = ReactiveCommand.CreateFromObservable(() 
                => Router.NavigateAndReset.Execute(new SettingsViewModel(this)));
            NewScan = ReactiveCommand.CreateFromObservable(() 
                => Router.NavigateAndReset.Execute(new EnterSampleReferenceViewModel(new ScanCaptureModel(), this)));

            NewScan.Execute();

            Router.NavigationChanged.Subscribe(x =>
            {
                this.TopBarViewModel.SetVisibilities(Observable.Latest(Router.CurrentViewModel).First());
            });
        }
    }
}
