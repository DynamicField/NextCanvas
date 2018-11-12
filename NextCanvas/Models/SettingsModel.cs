#region

using System.Collections.Generic;
using Ionic.Zlib;
using NextCanvas.ViewModels;

#endregion

namespace NextCanvas.Models
{
    public class SettingsModel
    {
        public CompressionLevel FileCompressionLevel { get; set; } = CompressionLevel.Level3;
        public List<object> DefaultValues { get; set; } = new List<object>();
    }
}