#region

using System;
using System.Windows.Ink;
using Newtonsoft.Json;
using NextCanvas.Ink;

#endregion

namespace NextCanvas.Models
{
    public abstract class StrokeTool<T> : StrokeTool where T : Stroke
    {
        [JsonIgnore]
        public override Type StrokeType => typeof(T);

        [JsonIgnore]
        public sealed override StrokeDelegate<Stroke> StrokeConstructor => StrokeImplementation;

        protected abstract StrokeDelegate<T> StrokeImplementation { get; }
    }

    public abstract class StrokeTool : Tool
    {
        [JsonIgnore]
        public abstract Type StrokeType { get; }
        [JsonIgnore]
        public abstract StrokeDelegate<Stroke> StrokeConstructor { get; }
    }
    public delegate T StrokeDelegate<out T>(Stroke stroke) where T : Stroke;
}