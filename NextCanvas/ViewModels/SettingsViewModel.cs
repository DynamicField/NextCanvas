#region

using Ionic.Zlib;
using NextCanvas.Models;
using NextCanvas.Properties;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Threading;
using System.Windows.Media;
using Fluent;

#endregion

namespace NextCanvas.ViewModels
{
    public class SettingsViewModel : ViewModelBase<SettingsModel>
    {
        public SettingsViewModel(SettingsModel model = null) : base(model)
        {
            DefaultValues = new ObservableModelCollection<object>(Model.DefaultValues);
            Groups = new ObservableViewModelCollection<ToolGroupViewModel, ToolGroup>(Model.Groups, g => new ToolGroupViewModel(g));
            Tools = new ObservableViewModelCollection<ToolViewModel, Tool>(Model.Tools, m =>
            {
                var viewModel = ToolViewModel.GetViewModel(m);
                var found = Groups.FirstOrDefault(g => g == viewModel.Group);
                if (found != null)
                    viewModel.Group = found;
                return viewModel;
            }, v =>
            {
                var found = Groups.FirstOrDefault(g => g == v.Group);
                if (found != null)
                    v.Group = found;
            });
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
            get => (int)Model.FileCompressionLevel;
            set
            {
                Model.FileCompressionLevel = (CompressionLevel)value;
                OnPropertyChanged(nameof(FileCompressionLevel));
            }
        }

        public bool IsRibbonOnTop
        {
            get => Model.IsRibbonOnTop;
            set
            {
                Model.IsRibbonOnTop = value;
                OnPropertyChanged(nameof(IsRibbonOnTop));
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
        public bool HasLanguage()
        {
            return !(Model.PreferredLanguage is null);
        }

        public CultureInfo PreferredLanguage
        {
            get => Model.PreferredLanguage ?? Thread.CurrentThread.CurrentUICulture;
            set
            {
                if (value == null || Equals(Model.PreferredLanguage, value)) return;
                ResourceSet previousResourceSet;
                try
                {
                    previousResourceSet =
                        DefaultObjectNamesResources.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
                }
                catch (MissingManifestResourceException)
                {
                    previousResourceSet = DefaultObjectNamesResources.ResourceManager.GetResourceSet(CultureInfo.GetCultureInfo("en"), true, true);
                }
                Model.PreferredLanguage = value;
                OnPropertyChanged(nameof(PreferredLanguage));
                Thread.CurrentThread.CurrentUICulture = value;
                var resourceSet = DefaultObjectNamesResources.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
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
        public ObservableViewModelCollection<ToolViewModel, Tool> Tools { get; set; }
        public ObservableViewModelCollection<ToolGroupViewModel, ToolGroup> Groups { get; set; }
        public ObservableCollection<Color> FavoriteColors => Model.FavoriteColors;
    }
}