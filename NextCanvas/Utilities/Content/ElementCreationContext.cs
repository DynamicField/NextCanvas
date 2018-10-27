using System;
using System.Windows.Controls;
using NextCanvas.Controls;

namespace NextCanvas.Utilities.Content
{
    public class ElementCreationContext
    {
        private readonly NextInkCanvas canvas;
        private readonly ScrollViewer scrollViewer;

        public ElementCreationContext(NextInkCanvas canvas, ScrollViewer scrollViewer)
        {
            this.canvas = canvas;
            this.scrollViewer = scrollViewer;
        }

        public SelectionWrapper Selection => canvas.SelectionHelper;
        public double ContentHorizontalOffset => scrollViewer.ContentHorizontalOffset;
        public double ContentVerticalOffset => scrollViewer.ContentVerticalOffset;
        public double VisibleWidth => Math.Min(scrollViewer.ActualWidth, canvas.Width);
        public double VisibleHeight => Math.Min(scrollViewer.ActualHeight, canvas.Height);
    }
}