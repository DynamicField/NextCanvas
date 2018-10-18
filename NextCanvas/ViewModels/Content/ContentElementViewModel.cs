using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextCanvas.Models.Content;

namespace NextCanvas.ViewModels.Content
{
    // oof that's one complicated class 😓
    public abstract class ContentElementViewModel<T> : ViewModelBase<T> where T : ContentElement, new()
    {
        public virtual double Left
        {
            get { return Model.Left; }
            set { Model.Left = value; OnPropertyChanged(nameof(Left)); }
        }

        public virtual double Top
        {
            get { return Model.Top; }
            set { Model.Top = value; OnPropertyChanged(nameof(Top)); }
        }

        public virtual double Bottom
        {
            get { return Model.Bottom; }
            set { Model.Bottom = value; OnPropertyChanged(nameof(Bottom)); }
        }

        public virtual double Width
        {
            get { return Model.Width; }
            set { Model.Width = value; OnPropertyChanged(nameof(Width)); }
        }

        public virtual double Height
        {
            get { return Model.Height; }
            set { Model.Height = value; OnPropertyChanged(nameof(Height)); }
        }
    }
}
