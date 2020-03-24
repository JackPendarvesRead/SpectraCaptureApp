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
using SpectraCaptureApp.ViewModel.Controls;

namespace SpectraCaptureApp.View.Controls
{
    /// <summary>
    /// Interaction logic for FileBrowse.xaml
    /// </summary>
    public partial class FileBrowse : ReactiveUserControl<FileBrowseViewModel>
    {
        public FileBrowse()
        {
            InitializeComponent();
        }
    }
}
