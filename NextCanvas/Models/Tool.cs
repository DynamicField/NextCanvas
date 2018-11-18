#region

using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using Newtonsoft.Json;

#endregion

namespace NextCanvas.Models
{
    public class Tool : INamedObject
    {
        private bool? hasDemo;

        public Tool()
        {
            DrawingAttributes.FitToCurve = true;
            DrawingAttributes.Color = Group.Color;
        }
        public DrawingAttributes DrawingAttributes { get; set; } = new DrawingAttributes();
        public bool HasColor { get; set; } = true;
        public string Name { get; set; } = "Tool";
        public object LargeIcon { get; set; }
        public object SmallIcon { get; set; }
        public ToolGroup Group { get; set; } = new ToolGroup();
        public Cursor Cursor { get; set; } = Cursors.Pen;

        public bool HasDemo
        {
            get => hasDemo ?? HasColor;
            set => hasDemo = value;
        }

        public bool HasSize => Mode != InkCanvasEditingMode.None && Mode != InkCanvasEditingMode.Select;
        public InkCanvasEditingMode Mode { get; set; } = InkCanvasEditingMode.Ink;
    }
}