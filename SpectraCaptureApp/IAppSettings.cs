using System;
using System.Collections.Generic;
using System.Text;

namespace SpectraCaptureApp
{
    interface IAppSettings<T>
        where T : class
    {
        T Load();
        void Save(T data);
    }
}
