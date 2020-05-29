using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace SpectraCaptureApp
{
    internal class SettingsManager<T>
        where T : class
    {
        private readonly string filePath;

        public SettingsManager(string fileName)
        {
            var directory = GetOrCreateDirectory("SpectraCaptureApp");
            filePath = Path.Combine(directory.FullName, fileName);
        }

        private DirectoryInfo GetOrCreateDirectory(string folderName)
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string directoryPath = Path.Combine(appData, folderName);
            return Directory.CreateDirectory(directoryPath);
        }

        public T LoadSettings()
        {
            if (File.Exists(filePath))
            {
                return JsonSerializer.Deserialize<T>(File.ReadAllText(filePath));
            }
            return null;
        }

        public void SaveSettings(T settings)
        {
            string json = JsonSerializer.Serialize(settings);
            File.WriteAllText(filePath, json);
        }
    }
}
