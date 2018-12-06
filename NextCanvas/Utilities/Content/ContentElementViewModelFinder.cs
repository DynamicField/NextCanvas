using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextCanvas.Content;
using NextCanvas.Content.ViewModels;

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
