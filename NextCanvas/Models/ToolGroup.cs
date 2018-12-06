#region

using System.Windows.Media;

#endregion

namespace NextCanvas
{
    public class ToolGroup
    {
        private bool? _hasDemo;
        public string Name { get; set; } = "Unknown";
        public bool HasGotColor { get; set; } = true;

        public bool HasDemo
        {
            get => _hasDemo ?? HasGotColor;
            set => _hasDemo = value;
        }

        public Color Color { get; set; } = Colors.Black;

        public override string ToString()
        {
            return Name;
        }
    }
}