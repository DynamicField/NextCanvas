using System;
using System.Windows.Ink;

namespace NextCanvas
{
    public interface IStrokeTool<out T> where T : Stroke
    {
        Type StrokeType { get; }
        StrokeDelegate<T> StrokeConstructor { get; }
    }
}