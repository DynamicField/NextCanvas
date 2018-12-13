using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NextCanvas.Utilities
{
    public static class ReflectionExtensions
    {
        public static string GetSimpleName(this Assembly assembly) => assembly.GetName().Name;
    }
}
