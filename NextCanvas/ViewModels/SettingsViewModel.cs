#region

using System.Linq;
using System.Windows.Media;
using Ionic.Zlib;
using NextCanvas.Models;

#endregion

namespace NextCanvas.ViewModels
{
    public class SettingsViewModel : ViewModelBase<SettingsModel>
    {
        public SettingsViewModel(SettingsModel model = null) : base(model)
        {
            DefaultValues = new ObservableModelCollection<object>(Model.DefaultValues);
        }
        public ObservableModelCollection<object> DefaultValues { get; }
        public T GetDefaultValue<T>() where T : class, new()
        {
            var tryResult = DefaultValues.OfType<T>().FirstOrDefault();
            if (tryResult != null) return tryResult;
            var itemToAdd = new T();
            DefaultValues.Add(itemToAdd);
            return itemToAdd;
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

        public FontFamily DefaultFontFamily
        {
            get => Model.DefaultFontFamily;
            set
            {
                Model.DefaultFontFamily = value;
                OnPropertyChanged(nameof(DefaultFontFamily));
            }
        }

        public double DefaultFontSize
        {
            get => Model.DefaultFontSize;
            set
            {
                Model.DefaultFontSize = value;
                OnPropertyChanged(nameof(DefaultFontSize));
            }
        }
    }
}