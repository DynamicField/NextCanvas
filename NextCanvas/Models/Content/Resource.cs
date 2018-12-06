#region

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

#endregion

namespace NextCanvas.Content
{
    public class Resource : IDisposable
    {
        [JsonIgnore] private Stream stream;

        public Resource()
        {
        }

        /// <summary>
        ///     Creates a resource with that name, and uses the stream to copy it inside the <see cref="Data" />
        /// </summary>
        /// <param name="name">The name, usually something like file.ext</param>
        /// <param name="data">The deeta. Oh no, my system crashed, i lost my deeta!</param>
        public Resource(string name, Stream data)
        {
            Name = name;
            if (data is MemoryStream memory)
            {
                Data = memory;
                return;
            }

            var memoryStream = new MemoryStream();
            data.Position = 0;
            data.CopyTo(memoryStream);
            Data = memoryStream;
        }

        public string Name { get; set; }
        public ResourceType Type { get; set; }

        /// <summary>
        ///     The deeta
        /// </summary>
        /// <remarks>It can be any sort of stream for convenience.</remarks>
        [JsonIgnore]
        public Stream Data
        {
            get => stream;
            set
            {
                stream = value;
                ComputeMD5();
            }
        }

        public string DataMD5Hash { get; set; }

        public void Dispose()
        {
            stream?.Dispose();
        }

        public static string ComputeMD5(Stream stream)
        {
            using (var md5 = new MD5CryptoServiceProvider())
            {
                stream.Position = 0;
                md5.Initialize();
                var hashBytes = md5.ComputeHash(stream);
                stream.Position = 0;
                // Convert the byte array to hexadecimal string
                // https://stackoverflow.com/questions/11454004/calculate-a-md5-hash-from-a-string
                var sb = new StringBuilder();
                foreach (var bytey in hashBytes) sb.Append(bytey.ToString("X2"));
                return sb.ToString();
            }
        }

        private void ComputeMD5()
        {
            DataMD5Hash = ComputeMD5(Data);
        }
    }
}