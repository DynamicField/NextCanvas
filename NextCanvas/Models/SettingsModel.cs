#region

using Ionic.Zlib;

#endregion

namespace NextCanvas.Models
{
    public class SettingsModel
    {
        public CompressionLevel FileCompressionLevel { get; set; } = CompressionLevel.Level3;
    }
}