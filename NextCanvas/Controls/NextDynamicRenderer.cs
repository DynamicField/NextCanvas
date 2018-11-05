using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Input.StylusPlugIns;
using System.Windows.Media;

namespace NextCanvas.Controls
{
    public class NextDynamicRenderer : DynamicRenderer
    {
        // ReSharper disable once RedundantOverriddenMember
        protected override void OnDraw(DrawingContext drawingContext, StylusPointCollection stylusPoints,
            Geometry geometry, Brush fillBrush)
        {
            base.OnDraw(drawingContext, stylusPoints, geometry, fillBrush);
        }
    }
}