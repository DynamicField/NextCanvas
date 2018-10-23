using System.Windows.Media;

namespace NextCanvas.Models
{
    public class ToolGroup
    {
        private bool? hasDemo;
        public string Name { get; set; } = "Unknown";
        public bool HasGotColor { get; set; } = true;

        public bool HasDemo
        {
            get => hasDemo ?? HasGotColor;
            set => hasDemo = value;
        }

        public Color Color { get; set; } = Colors.Black;

        public override string ToString()
        {
            return Name;
        }
    }
}