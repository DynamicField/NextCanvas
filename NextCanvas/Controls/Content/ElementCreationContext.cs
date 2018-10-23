namespace NextCanvas.Controls.Content
{
    public class ElementCreationContext
    {
        public SelectionWrapper Selection { get; }
        public double ContentHorizontalOffset { get; }
        public double ContentVerticalOffset { get; }
        public double VisibleWidth { get; }
        public double VisibleHeight { get; }

        public ElementCreationContext(
            SelectionWrapper s,
            double contentHorizontalOffset,
            double contentVerticalOffset,
            double visibleWidth,
            double visibleHeight)
        {
            Selection = s;
            ContentHorizontalOffset = contentHorizontalOffset;
            ContentVerticalOffset = contentVerticalOffset;
            VisibleWidth = visibleWidth;
            VisibleHeight = visibleHeight;
        }
    }
}