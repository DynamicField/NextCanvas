#region

using System.Windows.Ink;
using NextCanvas.Ink;

#endregion

namespace NextCanvas
{
    public class SquareTool : StrokeTool<SquareStroke>
    {
        protected override StrokeDelegate<SquareStroke> StrokeImplementation => GetStroke;

        private static SquareStroke GetStroke(Stroke s)
        {
            return new SquareStroke(s.StylusPoints, s.DrawingAttributes);
        }
    }
}