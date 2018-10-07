using System;
using System.IO;
using System.Windows.Ink;

namespace NextCanvas.Models
{
    public class Page
    {
        [Newtonsoft.Json.JsonIgnore]
        public StrokeCollection Strokes { get; set; } = new StrokeCollection();
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
        public int Width { get; set; } = 1080;
        public int Height { get; set; } = 720;
    }
}