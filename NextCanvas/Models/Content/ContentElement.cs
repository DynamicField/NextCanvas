namespace NextCanvas.Models.Content
{
    public class ContentElement
    {
        public double Left { get; set; } = 0;
        public double Top { get; set; } = 0;
        public double Bottom { get; set; } = 0;
        public double Right { get; set; } = 0;

        public virtual double Width { get; set; } = 200;
        public virtual double Height { get; set; } = 200;
    }
}
