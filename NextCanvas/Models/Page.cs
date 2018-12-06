#region

using System.Collections.Generic;
using System.Windows.Ink;
using Newtonsoft.Json;
using NextCanvas.Content;
using NextCanvas.Serialization;

#endregion

namespace NextCanvas
{
    public class Page
    {
        [JsonIgnore] public StrokeCollection Strokes { get; set; } = new StrokeCollection();
        
        public byte[] StrokesSerialized
        {
            get => StrokeSerializer.StrokesToBytes(Strokes);
            set => Strokes = StrokeSerializer.DeserializeStrokes(value);
        }

        public List<ContentElement> Elements { get; set; } = new List<ContentElement>();
        public int Width { get; set; } = 1920;
        public int Height { get; set; } = 1200;
    }
}