using NextCanvas.Models.Content;

namespace NextCanvas.ViewModels.Content
{
    public class TextBoxElementViewModel : ContentElementViewModel
    {
        public new TextBoxElement Model { get => (TextBoxElement)base.Model; }
        protected override ContentElement BuildDefaultModel()
        {
            return new TextBoxElement();
        }
        public TextBoxElementViewModel()
        {
        }
        public TextBoxElementViewModel(TextBoxElement model) : base(model)
        {
        }    
        public string RtfText
        {
            get { return Model.RtfText; }
            set { Model.RtfText = value; OnPropertyChanged(nameof(RtfText)); }
        }
        public void SaveRtf()
        {

        }
    }
}
