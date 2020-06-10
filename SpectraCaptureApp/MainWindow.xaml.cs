﻿using ReactiveUI;
using SpectraCaptureApp.ViewModel;
using System.Reactive.Disposables;

namespace SpectraCaptureApp
{
    public partial class MainWindow : ReactiveWindow<MainViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new MainViewModel();
            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, vm => vm.Router, view => view.RoutedViewHost.Router).DisposeWith(disposables);

                this.Bind(ViewModel, vm => vm.TopBarViewModel, view => view.TopBar.ViewModel).DisposeWith(disposables);
                
                this.BindCommand(ViewModel, vm => vm.SettingsNavigateCommand, view => view.TopBar.SettingsButton).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.NewScan, view => view.TopBar.NewScanButton).DisposeWith(disposables);

                this.Bind(ViewModel, vm => vm.TopBarViewModel.SpectrometerIsConnected, view => view.SpecConnectedCheck.IsChecked).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.TopBarViewModel.BaselineIsOk, view => view.BaselineCheck.IsChecked).DisposeWith(disposables);
            });
        }
    }
}
