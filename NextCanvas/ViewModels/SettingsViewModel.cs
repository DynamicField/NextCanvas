#region

using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Media;
using Ionic.Zlib;
using NextCanvas.Models;
using NextCanvas.Properties;

#endregion

namespace NextCanvas.ViewModels
{
    public class SettingsViewModel : ViewModelBase<SettingsModel>
    {
        public SettingsViewModel(SettingsModel model = null) : base(model)
        {
            DefaultValues = new ObservableModelCollection<object>(Model.DefaultValues);
            Tools = new ObservableViewModelCollection<ToolViewModel, Tool>(Model.Tools, ToolViewModel.GetViewModel);
            Groups = new ObservableViewModelCollection<ToolGroupViewModel, ToolGroup>(Model.Groups, g => new ToolGroupViewModel(g));
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

        public int MaxToolsDisplayed
        {
            get => Model.MaxToolsDisplayed;
            set
            {
                Model.MaxToolsDisplayed = value;
                OnPropertyChanged(nameof(MaxToolsDisplayed));
            }
        }

        public static event EventHandler CultureChanged;
        public bool HasLanguage() => Model.PreferredLanguage is null;
        public CultureInfo PreferredLanguage
        {
            get => Model.PreferredLanguage ?? Thread.CurrentThread.CurrentUICulture;
            set
            {
                if (value != null && !Equals(Model.PreferredLanguage, value))
                {
                    var previousResourceSet =
                        DefaultObjectNamesResources.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true,
                            true);
                    Model.PreferredLanguage = value;
                    OnPropertyChanged(nameof(PreferredLanguage));
                    Thread.CurrentThread.CurrentUICulture = value;
                    var resourceSet = DefaultObjectNamesResources.ResourceManager
                        .GetResourceSet(CultureInfo.CurrentUICulture, true, true);
                    foreach (var tool in Tools)
                    {
                        foreach (DictionaryEntry entry in previousResourceSet)
                        {
                            if (entry.Value.ToString() == tool.Name)
                            {
                                tool.Name = resourceSet.GetString(entry.Key.ToString());
                                break;
                            }
                        }
                    }
                    CultureChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        public ObservableViewModelCollection<ToolViewModel, Tool> Tools { get; set; }
        public ObservableViewModelCollection<ToolGroupViewModel, ToolGroup> Groups { get; set; }
    }
}