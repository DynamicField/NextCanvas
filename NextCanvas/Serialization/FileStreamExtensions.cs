using System.IO;
using NextCanvas.Models.Content;

namespace NextCanvas.Serialization
{
    public static class FileStreamExtensions
    {
        public static string GetMD5FromFile(this Stream stream)
        {
            return Resource.ComputeMD5(stream);
        }
    }
}