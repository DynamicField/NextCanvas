#region

using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using NextCanvas.Models;
using NextCanvas.ViewModels;

#endregion

namespace NextCanvas
{
    public static class SettingsManager
    {
        private static JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            PreserveReferencesHandling = PreserveReferencesHandling.All
        };
        static SettingsManager()
        {
            Directory.CreateDirectory(ApplicationDataPath);
            try
            {
                using (var streamReader = new StreamReader(FilePath, Encoding.UTF8))
                {
                    Settings = new SettingsViewModel(
                        JsonConvert.DeserializeObject<SettingsModel>(streamReader.ReadToEnd(), _serializerSettings));
                }
            }
            catch (Exception e)
            {
                // It's fine we will save it later
                Settings = new SettingsViewModel();
            }
        }

        private static string ApplicationDataPath =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\NextCanvas\\");
        private static string FilePath =>
            Path.Combine(ApplicationDataPath, "settings.json");

        public static SettingsViewModel Settings { get; }

        public static void SaveSettings()
        {          
            using (var file = new FileStream(FilePath, FileMode.Create, FileAccess.Write))
            {
                using (var streamWriter = new StreamWriter(file, Encoding.UTF8))
                {
                    streamWriter.WriteLine(JsonConvert.SerializeObject(Settings.Model, _serializerSettings));
                }
            }
        }
    }
}