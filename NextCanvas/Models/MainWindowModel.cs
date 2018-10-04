using System;
using System.Collections.Generic;
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
        public List<Tool> Tools { get; set; } = new List<Tool> {
            new Tool
            {
                Name = "Normal Brush",
                Group = "Brushes",
                LargeIcon = new Uri("pack://application:,,,/Images/Ribbon/Home/Brush.png"),
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
                Group = "Brushes",
                LargeIcon = new Uri("pack://application:,,,/Images/Ribbon/Home/Brush.png"),
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
                Group = "Brushes",
                LargeIcon = new Uri("pack://application:,,,/Images/Ribbon/Home/Brush.png"),
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
                Group = "Other",
                LargeIcon = new Uri("pack://application:,,,/Images/Ribbon/Home/Select.png"),
                Mode = System.Windows.Controls.InkCanvasEditingMode.Select,
                IsDisplayed = false
            },
            new Tool
            {
                Name = "Eraser",
                Group = "Other",
                LargeIcon = new Uri("pack://application:,,,/Images/Ribbon/Home/Eraser.png"),
                Mode = System.Windows.Controls.InkCanvasEditingMode.EraseByPoint,
                DrawingAttributes = new System.Windows.Ink.DrawingAttributes
                {
                    Width = 5,
                    Height = 5
                },
                Cursor = null
            },
            new Tool
            {
                Name = "Highlighter",
                Group = "Highlighters",
                LargeIcon = new Uri("pack://application:,,,/Images/Ribbon/Shared/Highlighter_24.png"),
                DrawingAttributes = new System.Windows.Ink.DrawingAttributes
                {
                    Color = Colors.Yellow,
                    Width = 2,
                    Height = 5,
                    IsHighlighter = true
                }
            }
        };
        public MainWindowModel() { }
    }
}
