using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextCanvas.Extensibility
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AddonElementAttribute : Attribute
    {
        public Type ViewModelType { get; }
        public AddonElementAttribute()
        {

        }
        public AddonElementAttribute(Type viewModelType)
        {
            ViewModelType = viewModelType;
        }
    }
}
