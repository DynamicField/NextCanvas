using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Ink;
using System.Windows.Input;

namespace NextCanvas.Ink
{
    // Parameter-less for binding yay :)
    class DemoStroke : Stroke
    {
        public DemoStroke() : this(new StylusPointCollection() { new StylusPoint(0, 0) }) { }
        public DemoStroke(StylusPointCollection stylusPoints) : base(stylusPoints)
        {
        }

        public DemoStroke(StylusPointCollection stylusPoints, DrawingAttributes drawingAttributes) : base(stylusPoints, drawingAttributes)
        {
        }
        
    }
}
