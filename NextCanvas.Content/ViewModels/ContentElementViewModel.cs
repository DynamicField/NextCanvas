
using NextCanvas.Content;
using NextCanvas.ViewModels;

namespace NextCanvas.Content.ViewModels
{
    // oof that's one complicated class 😓
    public class ContentElementViewModel : ViewModelBase<ContentElement>
    {
        public ContentElementViewModel() : this(null)
        {
        }

        public ContentElementViewModel(ContentElement model = null) : base(model)
        {
        }

        public virtual double Left
        {
            get => Model.Left;
            set
            {
                Model.Left = value;
                OnPropertyChanged(nameof(Left));
            }
        }

        public virtual double Top
        {
            get => Model.Top;
            set
            {
                Model.Top = value;
                OnPropertyChanged(nameof(Top));
            }
        }

        public virtual double Bottom
        {
            get => Model.Bottom;
            set
            {
                Model.Bottom = value;
                OnPropertyChanged(nameof(Bottom));
            }
        }

        public virtual double Right
        {
            get => Model.Right;
            set
            {
                Model.Right = value;
                OnPropertyChanged(nameof(Right));
            }
        }

        public virtual double Width
        {
            get => Model.Width;
            set
            {
                Model.Width = value;
                OnPropertyChanged(nameof(Width));
            }
        }

        public virtual double Height
        {
            get => Model.Height;
            set
            {
                Model.Height = value;
                OnPropertyChanged(nameof(Height));
            }
        }

        public int ZIndex
        {
            get => Model.ZIndex;
            set
            {
                Model.ZIndex = value;
                OnPropertyChanged(nameof(ZIndex));
            }
        }

        public static implicit operator ContentElement(ContentElementViewModel c) => c.Model;
    }
}