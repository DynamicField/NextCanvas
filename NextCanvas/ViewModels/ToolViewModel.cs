#region

using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using NextCanvas.Interactivity;

#endregion

namespace NextCanvas.ViewModels
{
    public class ToolViewModel : ViewModelBase<Tool>, INamedObject
    {
        private ToolGroupViewModel _group;
        public DelegateCommand ModifyCommand { get; private set; }

        public ToolViewModel(Tool model = null, Uri icon = null) : base(model)
        {
            Initialize();
            Model.DrawingAttributes.FitToCurve = true;
            if (icon == null) return;
            LargeIcon = icon;
        }

        public ToolViewModel() : this(null)
        {
        }

        public bool HasColor
        {
            get => Model.HasColor;
            set
            {
                Model.HasColor = value;
                OnPropertyChanged(nameof(HasColor));
                OnPropertyChanged(nameof(HasDemo));
            }
        }

        public string Name
        {
            get => Model.Name;
            set
            {
                Model.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public object LargeIcon
        {
            get => Model.LargeIcon;
            set
            {
                Model.LargeIcon = value;
                OnPropertyChanged(nameof(LargeIcon));
            }
        }

        public object SmallIcon
        {
            get => Model.SmallIcon ?? Model.LargeIcon;
            set
            {
                Model.SmallIcon = value;
                OnPropertyChanged(nameof(SmallIcon));
            }
        }

        public InkCanvasEditingMode Mode
        {
            get => Model.Mode;
            set
            {
                Model.Mode = value;
                OnPropertyChanged(nameof(Mode));
            }
        }

        public ToolGroupViewModel Group
        {
            get => _group;
            set
            {
                if (_group != null) _group.PropertyChanged -= Group_PropertyChanged;
                _group = value;
                Model.Group = _group.Model;
                _group.PropertyChanged += Group_PropertyChanged;
                OnPropertyChanged(nameof(Group));
                OnPropertyChanged(nameof(GroupName));
            }
        }

        public string GroupName => Group.Name;

        public virtual StrokeCollection DemoStroke => new StrokeCollection
        {
            new Stroke(new StylusPointCollection
            {
                new StylusPoint(15, 22),
                new StylusPoint(75, 22)
            }, DrawingAttributes)
        };
        private int _demoWidth = 90;

        public int DemoWidth
        {
            get => _demoWidth;
            set
            {
                _demoWidth = value;
                OnPropertyChanged(nameof(DemoWidth));
                OnPropertyChanged(nameof(DemoStroke));
            }
        }

        private int _demoHeight = 44;

        public int DemoHeight
        {
            get => _demoHeight;
            set
            {
                _demoHeight = value;
                OnPropertyChanged(nameof(DemoHeight));
                OnPropertyChanged(nameof(DemoStroke));
            }
        }
        public bool HasDemo
        {
            get => Model.HasDemo && Group.HasDemo;
            set
            {
                Model.HasDemo = value;
                OnPropertyChanged(nameof(HasDemo));
            }
        }

        public Cursor Cursor
        {
            get => Model.Cursor;
            set
            {
                Model.Cursor = value;
                OnPropertyChanged(nameof(Cursor));
                OnPropertyChanged(nameof(UseCursor));
            }
        }
        public bool UseCursor => Cursor != null; // ah yes is
        public bool HasSize => Mode != InkCanvasEditingMode.None && Mode != InkCanvasEditingMode.Select;
        public StylusShape EraserShape => new RectangleStylusShape(DrawingAttributes.Width, DrawingAttributes.Height);
        public bool IsCustomStrokeTool => this is StrokeToolViewModel;

        public DrawingAttributes DrawingAttributes
        {
            get
            {
                Model.DrawingAttributes.Color = Group.Color;
                return Model.DrawingAttributes;
            }
            set => InitDrawing(value);
        }

        public double UniformSize
        {
            get => DrawingAttributes.Height;
            set
            {
                DrawingAttributes.Height = value;
                if (!DrawingAttributes.IsHighlighter) DrawingAttributes.Width = value;
                OnPropertyChanged(nameof(UniformSize));
                OnPropertyChanged(nameof(EraserShape));
                // Some kind of hack to make the cursor update. dunno why microsoft didnt do gr8 xd
                UpdateCursorIfEraser(this);
            }
        }
        
        private void Initialize(ToolGroupViewModel group = null)
        {
            Group = group ?? new ToolGroupViewModel(Model.Group);
            InitDrawing(DrawingAttributes);
            ModifyCommand = new DelegateCommand(Modify);
        }

        private void Modify(object provider)
        {
            if (!(provider is IInteractionProvider<IModifyObjectInteraction> interact)) return;
            var modifyInteraction = interact.CreateInteraction();
            modifyInteraction.ObjectToModify = this;
            modifyInteraction.ShowInteraction();
        }
        private void Group_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ToolGroupViewModel.Name)) OnPropertyChanged(nameof(GroupName));
            if (e.PropertyName == nameof(ToolGroupViewModel.Color))
            {
                DrawingAttributes.Color = Group.Color;
                OnPropertyChanged(nameof(DrawingAttributes));
            }
        }

        private void InitDrawing(DrawingAttributes value)
        {
            Model.DrawingAttributes.AttributeChanged -= ColorChangeHandler;
            Model.DrawingAttributes = value;
            Model.DrawingAttributes.AttributeChanged += ColorChangeHandler;
            OnPropertyChanged(nameof(DrawingAttributes));
            OnPropertyChanged(nameof(DemoStroke));
        }

        private void ColorChangeHandler(object sender, PropertyDataChangedEventArgs e)
        {
            OnPropertyChanged(nameof(DemoStroke));
            if (e.NewValue is Color c && _group.Color != c) _group.Color = c;
        }

        public static void UpdateCursorIfEraser(ToolViewModel t)
        {
            if (t.Mode == InkCanvasEditingMode.EraseByPoint || t.Mode == InkCanvasEditingMode.EraseByStroke)
            {
                var previous = t.Mode;
                t.Mode = InkCanvasEditingMode.None;
                t.Mode = previous;
            }
        }
        public static ToolViewModel GetViewModel(Tool model)
        {
            switch (model)
            {
                case SquareTool s:
                    return new SquareToolViewModel(s);
                case StrokeTool st:
                    return new StrokeToolViewModel(st);
                default:
                    return new ToolViewModel(model);
            }
        }
    }
}