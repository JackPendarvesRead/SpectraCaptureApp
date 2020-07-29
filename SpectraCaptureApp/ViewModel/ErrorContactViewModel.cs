using ReactiveUI;
using SpectraCaptureApp.Logic;
using SpectraCaptureApp.Model;
using Splat;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;

namespace SpectraCaptureApp.ViewModel
{
    public class ErrorContactViewModel : ReactiveObject, IRoutableViewModel
    {
        public string UrlPathSegment => "Error";

        public ScanCaptureModel Model { get; }
        public IScreen HostScreen { get; }

        public ReactiveCommand<Unit,Unit> ExportErrorReportCommand { get; private set; }

        public ErrorContactViewModel(ScanCaptureModel model, IScreen screen = null)
        {
            Model = model;
            HostScreen = screen ?? Locator.Current.GetService<IScreen>();

            ExportErrorReportCommand = ReactiveCommand.Create(ExportErrorReportImpl);
            ExportErrorReportCommand.ThrownExceptions.Subscribe((ex) =>
            {
                MessageBox.Show(ex.Message);
            });
        }

        private void ExportErrorReportImpl()
        {
            var report = ErrorReportGenerator.GenerateDummy();
            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "Json File | *.json";
                if(sfd.ShowDialog() == DialogResult.OK)
                {
                    var json = JsonSerializer.Serialize(report, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(sfd.FileName, json);
                }
            }
        }
    }
}
