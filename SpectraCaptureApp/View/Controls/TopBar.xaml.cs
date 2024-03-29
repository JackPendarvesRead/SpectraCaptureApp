﻿using SpectraCaptureApp.ViewModel;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SpectraCaptureApp.ViewModel.Controls;

namespace SpectraCaptureApp.View.Controls
{
    /// <summary>
    /// Interaction logic for FileBrowse.xaml
    /// </summary>
    public partial class TopBar : ReactiveUserControl<TopBarViewModel>
    {
        public TopBar()
        {
            InitializeComponent();
            this.WhenActivated(disposables =>
            {
                //Commands
                this.BindCommand(ViewModel, vm => vm.SettingsNavigateCommand, view => view.SettingsButton).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.AbortCommand, view => view.AbortButton).DisposeWith(disposables);

                // Images
                this.OneWayBind(ViewModel, vm => vm.SpectrometerConnectedImageUri, view => view.SpectrometerConnectedImage.Source, GetImageSource).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.BaselineOkImageUri, view => view.BaselineOkImage.Source, GetImageSource).DisposeWith(disposables);

                // Visibility
                this.OneWayBind(ViewModel, vm => vm.AbortButtonVisible, view => view.AbortButton.Visibility).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.SettingsButtonVisible, view => view.SettingsButton.Visibility).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.BaselineOkImageVisible, view => view.BaselineOkImage.Visibility).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.BaselineOkImageVisible, view => view.BaselineOkText.Visibility).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.SpectrometerConnectedImageVisible, view => view.SpectrometerConnectedImage.Visibility).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.SpectrometerConnectedImageVisible, view => view.SpectrometerConnectedText.Visibility).DisposeWith(disposables);
            });
        }

        private ImageSource GetImageSource(string uriString)
        {
            return new BitmapImage(new Uri(uriString));
        }
    }
}
