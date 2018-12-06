using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextCanvas.Content.ViewModels;

namespace NextCanvas.Extensibility.Content
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ContentAddonElementAttribute : AddonElementAttribute
    {
        public ContentAddonElementAttribute()
        {
        }

        public ContentAddonElementAttribute(Type viewModelType)
        {
            ViewModelType = viewModelType;
            if (!viewModelType.IsSubclassOf(typeof(ContentElementViewModel)))
            {
                throw new InvalidOperationException("The view model type is not a ContentElementViewModel.");
            }
        }
        public Type ViewModelType { get; }
        public string Name { get; set; }
        public object Icon { get; set; }
        
    }
}
