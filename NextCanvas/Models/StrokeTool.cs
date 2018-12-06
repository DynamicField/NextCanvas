#region

using System;
using System.Windows.Ink;
using Newtonsoft.Json;

#endregion

namespace NextCanvas
{
    public abstract class StrokeTool<T> : StrokeTool, IStrokeTool<T> where T : Stroke
    {
        [JsonIgnore]
        public override Type StrokeType => typeof(T);

        [JsonIgnore]
        public new StrokeDelegate<T> StrokeConstructor => (StrokeDelegate<T>)base.StrokeConstructor;

        protected sealed override StrokeDelegate<Stroke> GetStrokeConstructor() => StrokeImplementation;

        protected abstract StrokeDelegate<T> StrokeImplementation { get; }
    }

    public abstract class StrokeTool : Tool
    {
        [JsonIgnore]
        public abstract Type StrokeType { get; }

        [JsonIgnore] public StrokeDelegate<Stroke> StrokeConstructor => GetStrokeConstructor();

        protected abstract StrokeDelegate<Stroke> GetStrokeConstructor();
    }
    public delegate T StrokeDelegate<out T>(Stroke stroke) where T : Stroke;
}