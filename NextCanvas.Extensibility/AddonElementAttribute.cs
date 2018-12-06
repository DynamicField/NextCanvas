using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextCanvas.Extensibility
{
    [AttributeUsage(AttributeTargets.Class)]
    public abstract class AddonElementAttribute : Attribute
    {
        public AddonElementAttribute()
        {

        }
    }
}
