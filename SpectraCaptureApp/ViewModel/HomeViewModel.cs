using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace SpectraCaptureApp.ViewModel
{
    public class HomeViewModel : ReactiveObject, IRoutableViewModel
    {
        public string UrlPathSegment => "Home";
        public IScreen HostScreen { get; }

        public HomeViewModel(IScreen screen = null)
        {
            HostScreen = screen ?? Locator.Current.GetService<IScreen>();            
        }
    }
}
