using System.Linq;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;

namespace NextCanvas.Ink
{
    public class SquareStroke : Stroke
    {
        public SquareStroke(StylusPointCollection stylusPoints, DrawingAttributes drawingAttributes) : base(
            stylusPoints, drawingAttributes.SetFitToCurve(false))
        {
            if (StylusPoints.Count > 3) PopulateRectangle(out _, out _, out _, out _);
        }

        //private Brush brush;
        //private Pen pen;
        public bool DisableSquare { get; set; } = true;

        protected override void DrawCore(DrawingContext drawingContext, DrawingAttributes drawingAttributes)
        {
            if (drawingAttributes.FitToCurve) drawingAttributes.FitToCurve = false; // no don't
            if (StylusPoints.Count < 2 || DisableSquare) base.DrawCore(drawingContext, drawingAttributes);
            //PopulateRectangle(out StylusPoint minX, out StylusPoint minY, out StylusPoint maxX, out StylusPoint maxY);
            //drawingContext.DrawRectangle(brush, pen, new Rect(new Point(minX.X, minY.Y), new Point(maxX.X, maxY.Y)));
        }

        private void PopulateRectangle(out StylusPoint minX, out StylusPoint minY, out StylusPoint maxX,
            out StylusPoint maxY, bool populate = true)
        {
            GetMinPoints(StylusPoints, out minX, out minY, out maxX, out maxY);
            var okPoints = new StylusPointCollection
            {
                new StylusPoint(minX.X, maxY.Y),
                new StylusPoint(minX.X, minY.Y),
                new StylusPoint(maxX.X, minY.Y),
                new StylusPoint(maxX.X, maxY.Y),
                new StylusPoint(minX.X, maxY.Y)
            };
            // top
            //for (int i = (int)minX.X; i < maxX.X; i++)
            //{
            //    okPoints.Add(new StylusPoint(i, maxY.Y));
            //}
            //bottom
            //for (int i = (int)minX.X; i < maxX.X; i++)
            //{
            //    okPoints.Add(new StylusPoint(i, minY.Y));
            //}
            //left
            //for (int i = (int)minY.Y; i < maxY.Y; i++)
            //{
            //    okPoints.Add(new StylusPoint(minX.X, i));
            //}
            //right
            //for (int i = (int)minY.Y; i < maxY.Y; i++)
            //{
            //    okPoints.Add(new StylusPoint(maxX.X, i));
            //}
            if (populate) StylusPoints = okPoints;
        }

        public static void GetMinPoints(StylusPointCollection c, out StylusPoint minX, out StylusPoint minY,
            out StylusPoint maxX, out StylusPoint maxY)
        {
            minX = c.OrderBy(t => t.X).First();
            minY = c.OrderBy(t => t.Y).First();
            maxX = c.OrderBy(t => t.X).Last();
            maxY = c.OrderBy(t => t.Y).Last();
        }
    }
}