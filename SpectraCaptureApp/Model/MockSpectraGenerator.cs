using Aunir.SpectrumAnalysis2.Core.Default;
using Aunir.SpectrumAnalysis2.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpectraCaptureApp.Model
{
    public static class MockSpectraGenerator
    {
        private static readonly int DataPoints = 100;
        private static readonly int Resolution = 2;
        private static readonly Random rng = new Random();

        public static ISpectrumData GenerateDarkSpectrum()
        {
            var wavelengths = new List<float>();
            var data = new List<float>();
            for (var i = 0; i < DataPoints; i++)
            {
                wavelengths.Add(100 + i * Resolution);
                data.Add(0);
            }
            return CreateSpectrumData(wavelengths, data);
        }

        public static ISpectrumData GenerateLightSpectrum()
        {
            var wavelengths = new List<float>();
            var data = new List<float>();
            for (var i = 0; i < DataPoints; i++)
            {
                wavelengths.Add(100 + i * Resolution);
                data.Add(20000);
            }
            return CreateSpectrumData(wavelengths, data);
        }

        public static ISpectrumData GenerateSubSampleSpectrum()
        {
            var wavelengths = new List<float>();
            var data = new List<float>();
            for (var i = 0; i < DataPoints; i++)
            {
                wavelengths.Add(100 + i * Resolution);
                data.Add(i + rng.Next(-10, 11));
            }
            return CreateSpectrumData(wavelengths, data);
        }

        private static ISpectrumData CreateSpectrumData(IList<float> wavelengths, IList<float> data)
        {
            return new SpectrumData(
                wavelengths,
                data,
                new Dictionary<string, string>(),
                "SampleRef",
                "InstrumentType",
                "InstrumentId",
                Aunir.SpectrumAnalysis2.Interfaces.Constants.XUnits.Nanometres,
                Aunir.SpectrumAnalysis2.Interfaces.Constants.YUnits.Counts);
        }
    }
}
