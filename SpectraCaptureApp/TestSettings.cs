using System;
using System.Collections.Generic;
using System.Text;

namespace SpectraCaptureApp
{
    public static class TestSettings
    {
        public static bool ThrowErrors { get; set; }
        public static bool SpectraIsValid { get; set; }
        public static bool BaselineOk { get; set; }

        public static int MaximumDarkCount => 100;
        public static int MinimumLightCount => 10000;
    }
}
