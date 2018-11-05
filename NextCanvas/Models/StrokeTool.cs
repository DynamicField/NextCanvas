using System;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using Newtonsoft.Json;

namespace NextCanvas.Models
{
    public abstract class StrokeTool<T> : Tool where T : Stroke
    {
        [JsonIgnore]
        public Type StrokeType => typeof(T);

        [JsonIgnore]
        public abstract StrokeDelegate<T> StrokeConstructor { get; }
    }

    public delegate T StrokeDelegate<out T>(Stroke stroke) where T : Stroke;
}