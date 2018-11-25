#region

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using Ionic.Zlib;
using Newtonsoft.Json;
using NextCanvas.Properties;

#endregion

namespace NextCanvas.Models
{
    public class SettingsModel
    {
        public SettingsModel()
        {
            Tools = new List<Tool>
            {
                new Tool
                {
                    Name = DefaultObjectNamesResources.Tools_MediumBrush,
                    Group = Groups[DefaultObjectNamesResources.ToolGroups_Brushes],
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
                    Name = DefaultObjectNamesResources.Tools_BigEraser,
                    Group = Groups[DefaultObjectNamesResources.ToolGroups_Erasers],
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
                    Name = DefaultObjectNamesResources.Tools_BigBrush,
                    Group = Groups[DefaultObjectNamesResources.ToolGroups_Brushes],
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
                    Name = DefaultObjectNamesResources.Tools_ThinBrush,
                    Group = Groups[DefaultObjectNamesResources.ToolGroups_Brushes],
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
                    Name = DefaultObjectNamesResources.Tools_Highlighter,
                    Group = Groups[DefaultObjectNamesResources.ToolGroups_Highlighters],
                    LargeIcon = new Uri(
                        "pack://application:,,,/NextCanvas;component/Images/Ribbon/Shared/Highlighter_24.png"),
                    DrawingAttributes = new DrawingAttributes
                    {
                        Color = Colors.Yellow,
                        Width = 3,
                        Height = 20,
                        IsHighlighter = true
                    }
                },
                new SquareTool
                {
                    Name = DefaultObjectNamesResources.Tools_Rectangle,
                    Group = Groups[DefaultObjectNamesResources.ToolGroups_Shapes],
                    LargeIcon = new Uri(
                        "pack://application:,,,/NextCanvas;component/Images/Ribbon/Home/Rectangle.png"),
                    DrawingAttributes = new DrawingAttributes
                    {
                        Width = 5,
                        Height = 5,
                        FitToCurve = false,
                        IgnorePressure = true
                    }
                },
                new Tool
                {
                    Name = DefaultObjectNamesResources.Tools_SmallEraser,
                    Group = Groups[DefaultObjectNamesResources.ToolGroups_Erasers],
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
                    Name = DefaultObjectNamesResources.Tools_MediumEraser,
                    Group = Groups[DefaultObjectNamesResources.ToolGroups_Erasers],
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
                    Name = DefaultObjectNamesResources.Tools_HugeEraser,
                    Group = Groups[DefaultObjectNamesResources.ToolGroups_Erasers],
                    LargeIcon = new Uri("pack://application:,,,/NextCanvas;component/Images/Ribbon/Home/Eraser.png"),
                    Mode = InkCanvasEditingMode.EraseByPoint,
                    DrawingAttributes = new DrawingAttributes
                    {
                        Width = 100,
                        Height = 100
                    },
                    Cursor = null
                }
            };
        }
        [JsonConstructor]
        public SettingsModel(List<Tool> tools)
        {
            Tools = tools;
        }
        public CompressionLevel FileCompressionLevel { get; set; } = CompressionLevel.Level3;
        public bool IsRibbonOnTop { get; set; } = false;
        public List<object> DefaultValues { get; set; } = new List<object>();
        public double DefaultFontSize { get; set; } = 16;

        public FontFamily DefaultFontFamily { get; set; } = Fonts.SystemFontFamilies.FirstOrDefault(t =>
            t.ToString().Equals("Calibri", StringComparison.InvariantCultureIgnoreCase));

        public int MaxToolsDisplayed { get; set; } = 6;
        public List<Tool> Tools { get; set; } 
        public CultureInfo PreferredLanguage { get; set; }
        [JsonProperty(Order = -2)]
        public ToolGroupCollection Groups { get; set; } = new ToolGroupCollection
        {
            new ToolGroup
            {
                Name = DefaultObjectNamesResources.ToolGroups_Brushes,
                Color = Colors.Black
            },
            new ToolGroup
            {
                Name = DefaultObjectNamesResources.ToolGroups_Highlighters,
                Color = Color.FromRgb(255, 222, 5)
            },
            new ToolGroup
            {
                Name = DefaultObjectNamesResources.ToolGroups_Erasers,
                Color = Colors.Black,
                HasGotColor = false
            },
            new ToolGroup
            {
                Name = DefaultObjectNamesResources.ToolGroups_Shapes,
                Color = Colors.Black
            },
        };

    }
}