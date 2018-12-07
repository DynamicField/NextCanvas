using NextCanvas.Content.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextCanvas.Extensibility.Content
{
    public static class AddonInfoContentExtensions
    {
        public static ContentElementViewModel CreateContentElement(this AddonElementData data, object model = null)
        {
            if (data.Type.IsSubclassOf(typeof(ContentElementAddon)))
            {
                return ContentElementAddon.CreateFromModel(data.Type, model);
            }
            if (!(data.Attribute is ContentAddonElementAttribute a) || a.ViewModelType == null) return null;
            if (model is null)
            {
                return (ContentElementViewModel) Activator.CreateInstance(a.ViewModelType);
            }
            return (ContentElementViewModel) Activator.CreateInstance(a.ViewModelType, model);
        }
        public static bool IsContentElement(this AddonElementData data)
        {
            return data.Attribute is ContentAddonElementAttribute;
        }
    }
}
