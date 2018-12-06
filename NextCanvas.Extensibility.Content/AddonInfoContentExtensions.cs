using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextCanvas.Content.ViewModels;

namespace NextCanvas.Extensibility.Content
{
    public static class AddonInfoContentExtensions
    {
        public static ContentElementViewModel CreateContentElement(this AddonElementData data)
        {
            if (data.Attribute is ContentAddonElementAttribute a)
            {
                return Activator.CreateInstance(a.ViewModelType ?? data.Type) as ContentElementViewModel;
            }
                return null;
        }
        public static ContentElementViewModel CreateContentElement(this AddonElementData data, object model)
        {
            if (data.Attribute is ContentAddonElementAttribute a)
            {
                return Activator.CreateInstance(a.ViewModelType ?? data.Type, model) as ContentElementViewModel;
            }
            return null;
        }

        public static bool IsContentElement(this AddonElementData data) =>
            data.Attribute is ContentAddonElementAttribute;
    }
}
