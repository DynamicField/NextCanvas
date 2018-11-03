using System;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using Newtonsoft.Json;
using NextCanvas.Ink;

namespace NextCanvas.Models
{
    public abstract class StrokeTool<T> : Tool where T : Stroke
    {
        [JsonIgnore]
        public Type StrokeType => typeof(T);

        [JsonIgnore]
        public abstract StrokeDelegate<T> StrokeConstructor { get; }
    }

    public class SquareTool : StrokeTool<SquareStroke>
    {
        public override StrokeDelegate<SquareStroke> StrokeConstructor => GetStroke;

        private static SquareStroke GetStroke(Stroke s)
        {
            return new SquareStroke(s.StylusPoints, s.DrawingAttributes);
        }
    }
    public delegate T StrokeDelegate<out T>(Stroke stroke) where T : Stroke;
}