using System.Linq;
using NextCanvas.Content;
using NextCanvas.Content.ViewModels;
using NextCanvas.Extensibility.Content;

namespace NextCanvas.Utilities.Content
{
    internal static class ContentElementViewModelFinder
    {
        public static ContentElementViewModel GetViewModel(ContentElement model)
        {
            switch (model)
            {
                case TextBoxElement t:
                    return new TextBoxElementViewModel(t);
                case ImageElement i:
                    return new ImageElementViewModel(i);
                case WebBrowserElement w:
                    return new WebBrowserElementViewModel(w);
                default:
                    return App.GetCurrent().Addons.SelectMany(a => a.ResolvedAddonElements)
                               .FirstOrDefault(a => a.IsContentElement())?.CreateContentElement(model) ??
                           new ContentElementViewModel(model);
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
