using System.Windows.Ink;

namespace NextCanvas.Ink
{
    public static class DrawingAttributesExtensions
    {
        public static DrawingAttributes SetFitToCurve(this DrawingAttributes t, bool fit)
        {
            t.FitToCurve = fit;
            return t;
        }
    }
}