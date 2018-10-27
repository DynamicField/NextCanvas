using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using NextCanvas.Models;
using NextCanvas.ViewModels;

namespace NextCanvas
{
    public static class SettingsManager
    {
        static SettingsManager()
        {
            try
            {
                using (var streamReader = new StreamReader(FilePath, Encoding.UTF8))
                {
                    Settings = new SettingsViewModel(
                        JsonConvert.DeserializeObject<SettingsModel>(streamReader.ReadToEnd()));
                }
            }
            catch (Exception)
            {
                // It's fine we will save it later
                Settings = new SettingsViewModel();
            }
        }

        private static string FilePath =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "NextCanvas\\settings.json");

        public static SettingsViewModel Settings { get; }

        public static void SaveSettings()
        {
            using (var file = File.Open(FilePath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                using (var streamWriter = new StreamWriter(file, Encoding.UTF8))
                {
                    streamWriter.WriteLine(JsonConvert.SerializeObject(Settings.Model));
                }
            }
        }
    }
}