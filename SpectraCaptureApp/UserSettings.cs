using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectraCaptureApp
{
    internal class UserSettings
    {
        public string SpectrumSaveDirectory { get; set; }
        public int FailedAttemptTries { get; set; }
    }
}
