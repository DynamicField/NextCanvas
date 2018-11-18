#region

using System.Windows.Media;
using NextCanvas.Models;

#endregion

namespace NextCanvas.ViewModels
{
    public class ToolGroupViewModel : ViewModelBase<ToolGroup>
    {
        public ToolGroupViewModel(ToolGroup model = null) : base(model)
        {
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

        public Color Color
        {
            get => Model.Color;
            set
            {
                Model.Color = value;
                OnPropertyChanged(nameof(Color));
            }
        }

        public bool HasGotColor
        {
            get => Model.HasGotColor;
            set
            {
                Model.HasGotColor = value;
                OnPropertyChanged(nameof(HasGotColor));
                OnPropertyChanged(nameof(HasDemo));
            }
        }

        public bool HasDemo
        {
            get => Model.HasDemo;
            set
            {
                Model.HasDemo = value;
                OnPropertyChanged(nameof(Color));
            }
        }
        public static bool operator ==(ToolGroupViewModel t, ToolGroupViewModel other) => t?.Name == other?.Name;
        public static bool operator !=(ToolGroupViewModel t, ToolGroupViewModel other) => t?.Name != other?.Name;
    }
}