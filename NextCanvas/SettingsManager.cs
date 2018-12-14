#region

using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Newtonsoft.Json;
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
            Task.Run(() =>
            {
                try
                {
                    using (var streamReader = new StreamReader(FilePath, Encoding.UTF8))
                    using (var jsonReader = new JsonTextReader(streamReader))
                    {
                        Settings = new SettingsViewModel(new JsonSerializer
                        {
                            TypeNameHandling = TypeNameHandling.Auto,
                            PreserveReferencesHandling = PreserveReferencesHandling.All
                        }.Deserialize<SettingsModel>(jsonReader));
                    }
                }
                catch (Exception)
                {
                    // It's fine we will save it later
                    Settings = new SettingsViewModel();
                }

                if (!Settings.FavoriteColors.Contains(Colors.Black))
                {
                    Settings.FavoriteColors.Add(Colors.Black);
                }
            });
        }

        internal static string ApplicationDataPath =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\NextCanvas\\");
        private static string FilePath =>
            Path.Combine(ApplicationDataPath, "settings.json");

        public static SettingsViewModel Settings { get; private set; } = new SettingsViewModel();
        
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