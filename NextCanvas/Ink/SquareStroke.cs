using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace NextCanvas.Ink
{
    public class SquareStroke : Stroke
    {
        private Brush brush;
        private Pen pen;
        public SquareStroke(System.Windows.Input.StylusPointCollection stylusPoints) : base(stylusPoints)
        {
            if (StylusPoints.Count > 3)
            {
                PopulateRectangle(out StylusPoint minX, out StylusPoint minY, out StylusPoint maxX, out StylusPoint maxY);
            }
            brush = new LinearGradientBrush(Colors.Red, Colors.Blue, 5d);
            pen = new Pen(brush, 2d);
            // lol
        }
        public bool DisableSquare { get; set; } = true;
        public SquareStroke(System.Windows.Input.StylusPointCollection stylusPoints, DrawingAttributes drawingAttributes) : base(stylusPoints, drawingAttributes)
        {
            brush = new LinearGradientBrush(Colors.Red, Colors.Blue, 5d);
            pen = new Pen(brush, 2d);
        }
        protected override void OnStylusPointsChanged(EventArgs e)
        {
            base.OnStylusPointsChanged(e);
        }
        protected override void DrawCore(DrawingContext drawingContext, DrawingAttributes drawingAttributes)
        {
            if (StylusPoints.Count < 2 || DisableSquare)
            {
                base.DrawCore(drawingContext, drawingAttributes);
                return;
            }
            PopulateRectangle(out StylusPoint minX, out StylusPoint minY, out StylusPoint maxX, out StylusPoint maxY);
            drawingContext.DrawRectangle(brush, pen, new Rect(new Point(minX.X, minY.Y), new Point(maxX.X, maxY.Y)));
        }

        private void PopulateRectangle(out StylusPoint minX, out StylusPoint minY, out StylusPoint maxX, out StylusPoint maxY, bool populate = true)
        {
            GetMinPoints(this.StylusPoints, out minX, out minY, out maxX, out maxY);
            StylusPointCollection okPoints = new StylusPointCollection
            {
                new StylusPoint(minX.X, maxY.Y),
                new StylusPoint(minX.X, minY.Y),
                new StylusPoint(maxX.X, minY.Y),
                new StylusPoint(maxX.X, maxX.Y),
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
            if (populate)
            {
                StylusPoints = okPoints;
            }
        }

        public static void GetMinPoints(StylusPointCollection c, out StylusPoint minX, out StylusPoint minY, out StylusPoint maxX, out StylusPoint maxY)
        {
            minX = c.OrderBy(t => t.X).First();
            minY = c.OrderBy(t => t.Y).First();
            maxX = c.OrderBy(t => t.X).Last();
            maxY = c.OrderBy(t => t.Y).Last();
        }
    }
}

