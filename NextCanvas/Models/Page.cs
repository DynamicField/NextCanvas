using System.Collections.Generic;
using System.IO;
using System.Windows.Ink;
using Newtonsoft.Json;
using NextCanvas.Models.Content;

namespace NextCanvas.Models
{
    public class Page
    {
        [JsonIgnore] public StrokeCollection Strokes { get; set; } = new StrokeCollection();

        public byte[] StrokesSerialized
        {
            get
            {
                using (var memory = new MemoryStream())
                {
                    Strokes.Save(memory);
                    return memory.ToArray();
                }
            }
            set
            {
                using (var memory = new MemoryStream(value))
                {
                    Strokes = new StrokeCollection(memory);
                }
            }
        }

        public List<ContentElement> Elements { get; set; } = new List<ContentElement>();
        public int Width { get; set; } = 1720;
        public int Height { get; set; } = 1420;
    }
}