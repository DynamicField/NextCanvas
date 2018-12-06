using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextCanvas.Extensibility.Content
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ContentAddonElementAttribute : AddonElementAttribute
    {
        public ContentAddonElementAttribute()
        {
        }

        public ContentAddonElementAttribute(Type viewModelType) : base(viewModelType)
        {
        }

        public string Name { get; set; }
        public object Icon { get; set; }
        
    }
}
