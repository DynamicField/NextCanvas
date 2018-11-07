#region

using NextCanvas.Models.Content;

#endregion

namespace NextCanvas.ViewModels.Content
{
    public class WebBrowserElementViewModel : ContentElementViewModel
    {
        public WebBrowserElementViewModel()
        {
            
        }

        public WebBrowserElementViewModel(WebBrowserElement model) : base(model)
        {
            
        }

        public string Url
        {
            get => Model.Url;
            set
            {
                Model.Url = value;
                OnPropertyChanged(nameof(Url));
            }
        }

        public new WebBrowserElement Model => (WebBrowserElement)base.Model;
        protected override ContentElement BuildDefaultModel() => new WebBrowserElement();
    }
}