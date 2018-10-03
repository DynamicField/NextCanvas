using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Input.StylusPlugIns;
using System.Windows.Media;
using System.Windows.Shapes;

namespace NextCanvas.Controls
{
    public class NextDynamicRenderer : DynamicRenderer
    {
        public NextDynamicRenderer()
        {
        }
        protected override void OnDraw(DrawingContext drawingContext, StylusPointCollection stylusPoints, Geometry geometry, Brush fillBrush)
        {
            base.OnDraw(drawingContext, stylusPoints, geometry, fillBrush);
        }
    }
}
