using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextCanvas.Extensibility
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class AddonInfoAttribute : Attribute
    {
        public string Name { get; }

        public AddonInfoAttribute(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }
    }
}
