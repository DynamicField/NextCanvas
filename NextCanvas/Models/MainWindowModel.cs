using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using Fluent;
using Newtonsoft.Json;

namespace NextCanvas.Models
{
    /// <summary>
    /// This is the main window model *SWOOSH*
    /// </summary>
    public class MainWindowModel
    {
        [JsonIgnore]
        public Document Document { get; set; } = new Document();
        public ToolGroupCollection Groups { get; set; } = new ToolGroupCollection
        {
            new ToolGroup
            {
                Name = "Brushes",
                Color = Colors.Black
            },

            new ToolGroup
            {
                Name = "Highlighters",
                Color = Color.FromRgb(255, 222, 5)
            },
            new ToolGroup
            {
                Name = "Erasers",
                Color = Colors.Black,
                HasGotColor = false
            },
            new ToolGroup
            {
                Name = "Other",
                Color = Colors.Black
            }
        };
        public ObservableCollection<Color> FavouriteColors { get; set; } = ColorGallery.RecentColors;
        public List<Tool> Tools { get; set; }
        public MainWindowModel()
        {
            Tools = new List<Tool> {
            new Tool
            {
                Name = "Normal Brush",
                Group = Groups["Brushes"],
                LargeIcon = new Uri("pack://application:,,,/NextCanvas;component/Images/Ribbon/Home/Brush.png"),
                DrawingAttributes = new DrawingAttributes
                {
                    Color = Colors.Black,
                    Height = 2,
                    Width = 2
                },
                Cursor = Cursors.Pen
            },
            new Tool
            {
                Name = "Medium Brush",
                Group = Groups["Brushes"],
                LargeIcon = new Uri("pack://application:,,,/NextCanvas;component/Images/Ribbon/Home/Brush.png"),
                DrawingAttributes = new DrawingAttributes
                {
                    Color = Colors.Black,
                    Height = 6,
                    Width = 6
                }
            },
            new Tool
            {
                Name = "Big Brush",
                Group = Groups["Brushes"],
                LargeIcon = new Uri("pack://application:,,,/NextCanvas;component/Images/Ribbon/Home/Brush.png"),
                DrawingAttributes = new DrawingAttributes
                {
                    Color = Colors.Black,
                    Height = 11,
                    Width = 11
                }
            },

            new Tool
            {
                Name = "Select",
                Group = Groups["Other"],
                LargeIcon = new Uri("pack://application:,,,/NextCanvas;component/Images/Ribbon/Home/Select.png"),
                Mode = InkCanvasEditingMode.Select,
                HasDemo = false,
                Cursor = null
            },
            new Tool
            {
                Name = "Small Eraser",
                Group = Groups["Erasers"],
                LargeIcon = new Uri("pack://application:,,,/NextCanvas;component/Images/Ribbon/Home/Eraser.png"),
                Mode = InkCanvasEditingMode.EraseByPoint,
                DrawingAttributes = new DrawingAttributes
                {
                    Width = 4,
                    Height = 4
                },
                Cursor = null
            },
            new Tool
            {
                Name = "Medium Eraser",
                Group = Groups["Erasers"],
                LargeIcon = new Uri("pack://application:,,,/NextCanvas;component/Images/Ribbon/Home/Eraser.png"),
                Mode = InkCanvasEditingMode.EraseByPoint,
                DrawingAttributes = new DrawingAttributes
                {
                    Width = 12,
                    Height = 12
                },
                Cursor = null
            },
            new Tool
            {
                Name = "Big Eraser",
                Group = Groups["Erasers"],
                LargeIcon = new Uri("pack://application:,,,/NextCanvas;component/Images/Ribbon/Home/Eraser.png"),
                Mode = InkCanvasEditingMode.EraseByPoint,
                DrawingAttributes = new DrawingAttributes
                {
                    Width = 40,
                    Height = 40
                },
                Cursor = null
            },
            new Tool
            {
                Name = "Huge Eraser",
                Group = Groups["Erasers"],
                LargeIcon = new Uri("pack://application:,,,/NextCanvas;component/Images/Ribbon/Home/Eraser.png"),
                Mode = InkCanvasEditingMode.EraseByPoint,
                DrawingAttributes = new DrawingAttributes
                {
                    Width = 100,
                    Height = 100
                },
                Cursor = null
            },
            new Tool
            {
                Name = "Highlighter",
                Group = Groups["Highlighters"],
                LargeIcon = new Uri("pack://application:,,,/NextCanvas;component/Images/Ribbon/Shared/Highlighter_24.png"),
                DrawingAttributes = new DrawingAttributes
                {
                    Color = Colors.Yellow,
                    Width = 3,
                    Height = 20,
                    IsHighlighter = true
                }
            }
        };
            if (!FavouriteColors.Contains(Colors.Black))
            {
                FavouriteColors.Add(Colors.Black);               
            }
        }
    }
}
