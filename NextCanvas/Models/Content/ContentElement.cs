namespace NextCanvas.Models.Content
{
    public class ContentElement
    {
        public double Left { get; set; }
        public double Top { get; set; }
        public double Bottom { get; set; }
        public double Right { get; set; }

        public double Width { get; set; } = 200;
        public double Height { get; set; } = 200;
    }
}