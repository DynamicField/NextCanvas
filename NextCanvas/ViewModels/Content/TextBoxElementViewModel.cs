#region

using NextCanvas.Models.Content;

#endregion

namespace NextCanvas.ViewModels.Content
{
    public class TextBoxElementViewModel : ContentElementViewModel, IViewModel<TextBoxElement>, INamedObject
    {
        public TextBoxElementViewModel(TextBoxElement model = null) : base(model)
        {
        }

        public TextBoxElementViewModel() : this(null)
        {
        }

        public string RtfText
        {
            get => Model.RtfText;
            set
            {
                Model.RtfText = value;
                OnPropertyChanged(nameof(RtfText));
            }
        }

        public new TextBoxElement Model => (TextBoxElement) base.Model;

        protected override ContentElement BuildDefaultModel()
        {
            return new TextBoxElement();
        }

        public string Name => "Text block";
    }
}