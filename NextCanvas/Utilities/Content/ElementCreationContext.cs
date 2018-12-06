#region

using System;
using System.Windows.Controls;
using NextCanvas.Controls;

#endregion

namespace NextCanvas.Utilities.Content
{
    public class ElementCreationContext
    {
        private readonly NextInkCanvas _canvas;
        private readonly ScrollViewer _scrollViewer;

        public ElementCreationContext(NextInkCanvas canvas, ScrollViewer scrollViewer)
        {
            this._canvas = canvas;
            this._scrollViewer = scrollViewer;
        }

        public SelectionWrapper Selection => _canvas.SelectionHelper;
        public double ContentHorizontalOffset => _scrollViewer.ContentHorizontalOffset;
        public double ContentVerticalOffset => _scrollViewer.ContentVerticalOffset;
        public double VisibleWidth => Math.Min(_scrollViewer.ActualWidth, _canvas.Width);
        public double VisibleHeight => Math.Min(_scrollViewer.ActualHeight, _canvas.Height);
    }
}