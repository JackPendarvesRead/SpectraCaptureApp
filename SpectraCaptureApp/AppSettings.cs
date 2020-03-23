using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectraCaptureApp
{
    internal static class AppSettings
    {
        public static string SpectrumSaveDirectory => ConfigurationManager.AppSettings["SpectrumSaveDirectory"];
    }
}
