#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Ionic.Zlib;

#endregion

namespace NextCanvas.Models
{
    public class SettingsModel
    {
        public CompressionLevel FileCompressionLevel { get; set; } = CompressionLevel.Level3;
        public List<object> DefaultValues { get; set; } = new List<object>();
        public double DefaultFontSize { get; set; } = 16;

        public FontFamily DefaultFontFamily { get; set; } = Fonts.SystemFontFamilies.FirstOrDefault(t =>
            t.ToString().Equals("Calibri", StringComparison.InvariantCultureIgnoreCase));
    }
}