using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace NextCanvas.Models
{
    public class ToolGroup
    {
        public string Name { get; set; } = "Unknown";
        public bool HasGotColor { get; set; } = true;
        public override string ToString()
        {
            return Name;
        }
        public Color Color { get; set; } = Colors.Black;
    }
}
