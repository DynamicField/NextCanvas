using System;
using System.Windows.Ink;

namespace NextCanvas.Models
{
    public class Page
    {
        public StrokeCollection Strokes { get; set; } = new StrokeCollection();
        public int Width { get; set; } = 1080;
        public int Height { get; set; } = 720;
    }
}