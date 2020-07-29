using SpectraCaptureApp.ViewModel;
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

namespace SpectraCaptureApp.View
{
    public partial class ErrorContactView : ReactiveUserControl<ErrorContactViewModel>
    {
        public ErrorContactView()
        {
            InitializeComponent();
            this.WhenActivated(disposables =>
            {
                this.Bind(ViewModel, vm => vm.Model.SampleReference, view => view.SampleReferenceText.Text).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.Comments, view => view.CommentsTextBox.Text).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.SampleType, view => view.SampleTypeTextBox.Text).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.UserName, view => view.UserNameTextBox.Text).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.ExportErrorReportCommand, view => view.ExportErrorReportButton).DisposeWith(disposables);
            });
        }
    }
}
