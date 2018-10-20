using System.Windows.Media;

namespace NextCanvas.Models
{
    public class ToolGroup
    {
        public string Name { get; set; } = "Unknown";
        public bool HasGotColor { get; set; } = true;
        private bool? hasDemo;

        public bool HasDemo
        {
            get => hasDemo ?? HasGotColor;
            set => hasDemo = value;
        }

        public override string ToString()
        {
            return Name;
        }
        public Color Color { get; set; } = Colors.Black;
    }
}
