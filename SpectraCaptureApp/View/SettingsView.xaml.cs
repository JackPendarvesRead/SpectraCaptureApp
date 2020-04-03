using ReactiveUI;
using SpectraCaptureApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SpectraCaptureApp.View
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : ReactiveUserControl<SettingsViewModel>
    {
        public SettingsView()
        {
            InitializeComponent();
            this.WhenActivated(disposables =>
            {
                this.BindCommand(ViewModel,
                    vm => vm.SaveDirectoryBrowseCommand,
                    view => view.SaveDirectoryFileBrowse.FileBrowseButton)
                .DisposeWith(disposables);

                this.SaveDirectoryFileBrowse.FileBrowseLabel.Content = "Bla bla bla bla";


            });
        }
    }
}
