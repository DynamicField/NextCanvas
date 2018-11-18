#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using Fluent;
using Newtonsoft.Json;

#endregion

namespace NextCanvas.Models
{
    /// <summary>
    ///     This is the main window model *SWOOSH*
    /// </summary>
    public class MainWindowModel
    {
        public MainWindowModel()
        {
            if (!FavouriteColors.Contains(Colors.Black)) FavouriteColors.Add(Colors.Black);
        }

        [JsonIgnore] public Document Document { get; set; } = new Document();

        public ObservableCollection<Color> FavouriteColors { get; set; } = ColorGallery.RecentColors;
        public List<Tool> Tools { get; set; }
    }
}