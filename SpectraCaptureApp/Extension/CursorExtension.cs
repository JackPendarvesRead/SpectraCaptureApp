using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SpectraCaptureApp.Extension
{
    public static class CursorExtension
    {
        public static IDisposable SetBusyCursor(this IObservable<bool> observable)
        {
            return observable.Subscribe(x =>
            {
                Mouse.OverrideCursor = x ? Cursors.Wait : null;
            });
        }
    }
}
