using System.IO;
using Newtonsoft.Json;

namespace NextCanvas.Models.Content
{
    public class Resource
    {
        public string Name { get; set; }
        public ResourceType Type { get; set; }
        /// <summary>
        /// The deeta
        /// </summary>
        /// <remarks>It can be any sort of stream for convenience.</remarks>
        [JsonIgnore]
        public Stream Data { get; set; }
    }
}
