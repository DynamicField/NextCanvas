using System.Windows.Ink;
using NextCanvas.Ink;

namespace NextCanvas.Models
{
    public class SquareTool : StrokeTool<SquareStroke>
    {
        public override StrokeDelegate<SquareStroke> StrokeConstructor => GetStroke;

        private static SquareStroke GetStroke(Stroke s)
        {
            return new SquareStroke(s.StylusPoints, s.DrawingAttributes);
        }
    }
}