using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
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
