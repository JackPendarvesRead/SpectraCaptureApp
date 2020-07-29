using NIR4.ViaviCapture.Model;
using ReactiveUI;
using Serilog;
using SpectraCaptureApp.Logic;
using SpectraCaptureApp.Model;
using Splat;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        private string comments;
        public string Comments
        {
            get => comments;
            set => this.RaiseAndSetIfChanged(ref comments, value);
        }

        private string sampleType;
        public string SampleType
        {
            get => sampleType;
            set => this.RaiseAndSetIfChanged(ref sampleType, value);
        }

        private string userName;
        public string UserName
        {
            get => userName;
            set => this.RaiseAndSetIfChanged(ref userName, value);
        }

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
            Log.Debug("ExportErrorReportCommand executing");
            var scanContext = Model.ScanningWorkflow.GetViaviScanContext();
            Log.Debug("ScanContext retrieved successfully");
            var report = new ErrorReport
            {
                Comments = this.Comments,
                DateGenerated = DateTime.UtcNow,
                ExceptionsThrownInformation = Model.WorkflowExceptions.Select(e => e.Message).ToList(),
                SampleType = this.SampleType,
                ScanContext = scanContext,
                User = this.UserName
            };
            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "Json File | *.json";
                sfd.FileName = Model.SampleReference;
                if(sfd.ShowDialog() == DialogResult.OK)
                {
                    Log.Debug("Attempting to export to {FilePath}", sfd.FileName);
                    var json = JsonSerializer.Serialize(report, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(sfd.FileName, json);
                    Log.Debug("File exported successfully");
                }
                else
                {
                    Log.Debug("Export cancelled in SaveFileDialog");
                }
            }
        }
    }
}
