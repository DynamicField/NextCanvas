using NextCanvas.Models.Content;
using NextCanvas.Utilities.Content;

namespace NextCanvas.ViewModels.Content
{
    // oof that's one complicated class 😓
    public class ContentElementViewModel : ViewModelBase<ContentElement>
    {
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

        public ContentElementViewModel()
        {
        }

        public ContentElementViewModel(ContentElement model) : base(model)
        {
        }

        public static ContentElementViewModel GetViewModel(ContentElement model)
        {
            switch (model)
            {
                case TextBoxElement t:
                    return new TextBoxElementViewModel(t);
                case ImageElement i:
                    return new ImageElementViewModel(i);
                default:
                    return new ContentElementViewModel(model);
            }
        }

        internal static ContentElementViewModel GetViewModel(ContentElement model, IResourceLocator locator)
        {
            ContentElementViewModel tempReturnValue;
            switch (model)
            {
                case ImageElement i:
                    tempReturnValue = new ImageElementViewModel(i, locator);
                    break;
                default:
                    tempReturnValue = null;
                    break;
            }

            return tempReturnValue ?? GetViewModel(model);
        }
    }
}