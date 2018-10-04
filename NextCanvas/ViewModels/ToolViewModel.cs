using NextCanvas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;

namespace NextCanvas.ViewModels
{
    public class ToolViewModel : ViewModelBase<Tool>
    {
        public ToolViewModel()
        {
        }
        public ToolViewModel(Tool model) : base(model)
        {
            model.DrawingAttributes.FitToCurve = true;
        }
        public ToolViewModel(Tool model, Uri icon) : this(model)
        {
            LargeIcon = icon;
        }

        public bool HasColor
        {
            get => Model.HasColor;
            set { Model.HasColor = value; OnPropertyChanged(nameof(HasColor)); }
        }
        public string Name
        {
            get => Model.Name;
            set { Model.Name = value; OnPropertyChanged(nameof(Name)); }
        }

        public object LargeIcon
        {
            get => Model.LargeIcon;
            set { Model.LargeIcon = value; OnPropertyChanged(nameof(LargeIcon)); }
        }
        public object SmallIcon
        {
            get => Model.SmallIcon;
            set { Model.SmallIcon = value; OnPropertyChanged(nameof(SmallIcon)); }
        }
        public InkCanvasEditingMode Mode
        {
            get => Model.Mode;
            set { Model.Mode = value; OnPropertyChanged(nameof(Mode)); }
        }
        public string Group
        {
            get => Model.Group;
            set { Model.Group = value; OnPropertyChanged(nameof(Group)); }
        }
        public bool IsDisplayed
        {
            get => Model.IsDisplayed;
            set { Model.IsDisplayed = value; OnPropertyChanged(nameof(IsDisplayed)); }
        }
        public Cursor Cursor
        {
            get => Model.Cursor;
            set { Model.Cursor = value; OnPropertyChanged(nameof(Cursor)); OnPropertyChanged(nameof(UseCursor)); }
        }
        public bool UseCursor => !(Cursor is null); // ah yes is
        public bool HasSize => Mode != InkCanvasEditingMode.None && Mode != InkCanvasEditingMode.Select;
        public StylusShape EraserShape => new RectangleStylusShape(DrawingAttributes.Width, DrawingAttributes.Height);
        public DrawingAttributes DrawingAttributes
        {
            get => Model.DrawingAttributes;
            set
            {
                Model.DrawingAttributes = value;
                OnPropertyChanged(nameof(DrawingAttributes));
            }
        }
        public double UniformSize
        {
            get => DrawingAttributes.Height;
            set
            {
                DrawingAttributes.Height = value;
                if (!DrawingAttributes.IsHighlighter)
                {
                    DrawingAttributes.Width = value;
                }
                OnPropertyChanged(nameof(UniformSize));
                OnPropertyChanged(nameof(EraserShape));
                // Some kind of hack to make the cursor update. dunno why microsoft didnt do gr8 xd
                if (Mode == InkCanvasEditingMode.EraseByPoint || Mode == InkCanvasEditingMode.EraseByStroke)
                {
                    InkCanvasEditingMode previous = Mode;
                    Mode = InkCanvasEditingMode.None;
                    Mode = previous;
                }
            }
        }
    }
}
