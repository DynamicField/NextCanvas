using NextCanvas.Models.Content;

namespace NextCanvas.ViewModels.Content
{
    // oof that's one complicated class 😓
    public class ContentElementViewModel : ViewModelBase<ContentElement>
    {

        public ContentElementViewModel()
        {
        }

        public ContentElementViewModel(ContentElement model) : base(model)
        {
        }
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

        public virtual double Right
        {
            get { return Model.Right; }
            set { Model.Right = value; OnPropertyChanged(nameof(Right)); }
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
        public static ContentElementViewModel GetViewModel(ContentElement model)
        {
            switch (model)
            {
                case TextBoxElement t:
                    return new TextBoxElementViewModel(t);
                default:
                    return new ContentElementViewModel(model);
            }
        }
    }
}
