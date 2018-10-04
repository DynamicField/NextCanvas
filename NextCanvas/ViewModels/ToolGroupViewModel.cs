using NextCanvas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace NextCanvas.ViewModels
{
    public class ToolGroupViewModel : ViewModelBase<ToolGroup>
    {
        public ToolGroupViewModel()
        {
        }

        public ToolGroupViewModel(ToolGroup model) : base(model)
        {
        }

        public string Name
        {
            get => Model.Name;
            set { Model.Name = value; OnPropertyChanged(nameof(Name)); }
        }
        public Color Color
        {
            get => Model.Color;
            set { Model.Color = value; OnPropertyChanged(nameof(Color)); }
        }
    }
}
