using System;
using System.Windows.Input;
using System.Windows.Media;

namespace NextCanvas.Models
{
    public abstract class StrokeTool : Tool
    {
        public abstract Type StrokeType { get; }

        public abstract void DynamicRendererDraw(
            DrawingContext drawingContext,
            StylusPointCollection stylusPoints,
            Geometry geometry,
            Brush fillBrush);
    }
}