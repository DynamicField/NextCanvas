using Fluent;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace NextCanvas.Models
{
    public class MainWindowModel
    {
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
                Color = Colors.Yellow
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
                DrawingAttributes = new System.Windows.Ink.DrawingAttributes
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
                DrawingAttributes = new System.Windows.Ink.DrawingAttributes
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
                DrawingAttributes = new System.Windows.Ink.DrawingAttributes
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
                Mode = System.Windows.Controls.InkCanvasEditingMode.Select,
                IsDisplayed = false,
                Cursor = null
            },
            new Tool
            {
                Name = "Small Eraser",
                Group = Groups["Erasers"],
                LargeIcon = new Uri("pack://application:,,,/NextCanvas;component/Images/Ribbon/Home/Eraser.png"),
                Mode = System.Windows.Controls.InkCanvasEditingMode.EraseByPoint,
                DrawingAttributes = new System.Windows.Ink.DrawingAttributes
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
                Mode = System.Windows.Controls.InkCanvasEditingMode.EraseByPoint,
                DrawingAttributes = new System.Windows.Ink.DrawingAttributes
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
                Mode = System.Windows.Controls.InkCanvasEditingMode.EraseByPoint,
                DrawingAttributes = new System.Windows.Ink.DrawingAttributes
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
                Mode = System.Windows.Controls.InkCanvasEditingMode.EraseByPoint,
                DrawingAttributes = new System.Windows.Ink.DrawingAttributes
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
                DrawingAttributes = new System.Windows.Ink.DrawingAttributes
                {
                    Color = Colors.Yellow,
                    Width = 2,
                    Height = 5,
                    IsHighlighter = true
                }
            }
        };
            FavouriteColors.Add(Colors.Black);
        }
    }
}
