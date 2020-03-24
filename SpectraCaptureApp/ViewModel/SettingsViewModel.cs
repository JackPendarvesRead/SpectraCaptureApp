using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using System.Windows.Forms;
using Splat;

namespace SpectraCaptureApp.ViewModel
{
    public class SettingsViewModel : ReactiveObject
    {
        public SettingsViewModel()
        {
            ReactiveCommand.Create(SaveDirectoryBrowseCommandImpl);
        }

        private void SaveDirectoryBrowseCommandImpl()
        {
            using(var sfd = new SaveFileDialog())
            {
                if(sfd.ShowDialog() == DialogResult.OK)
                {
                    var manager = Locator.Current.GetService<SettingsManager<AppSettings>>();
                   //manager.SaveSettings();
                }
            }
        }

        public ReactiveCommand<Unit, Unit> SaveDirectoryBrowseCommand { get; set; }

        public string SaveDirectory { get; set; }
    }
}
