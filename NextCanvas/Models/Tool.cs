using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;

namespace NextCanvas.Models
{
    public class Tool
    {
        public DrawingAttributes DrawingAttributes { get; set; } = new DrawingAttributes();
        public bool HasColor { get; set; } = true;
        public string Name { get; set; } = "Tool";
        public object LargeIcon { get; set; } = null;
        public object SmallIcon { get; set; } = null;
        public string Group { get; set; } = "Unknown";
        public Cursor Cursor { get; set; } = Cursors.Pen;
        public bool IsDisplayed { get; set; } = true;
        public InkCanvasEditingMode Mode { get; set; } = InkCanvasEditingMode.Ink;
        public Tool()
        {
            DrawingAttributes.FitToCurve = true;
        }
    }
    public abstract class StrokeTool : Tool
    {
        public abstract Type StrokeType { get; }
        public abstract void DynamicRendererDraw(DrawingContext drawingContext, StylusPointCollection stylusPoints, Geometry geometry, Brush fillBrush);
    }
}
