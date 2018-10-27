using Ionic.Zlib;
using NextCanvas.Models;

namespace NextCanvas.ViewModels
{
    public class SettingsViewModel : ViewModelBase<SettingsModel>
    {
        public SettingsViewModel()
        {
        }

        public SettingsViewModel(SettingsModel model) : base(model)
        {
        }

        public int FileCompressionLevel
        {
            get => (int) Model.FileCompressionLevel;
            set
            {
                Model.FileCompressionLevel = (CompressionLevel) value;
                OnPropertyChanged(nameof(FileCompressionLevel));
            }
        }
    }
}