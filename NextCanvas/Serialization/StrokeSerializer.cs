using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Ink;
using NextCanvas.Properties;

namespace NextCanvas.Serialization
{
    public static class StrokeSerializer
    {
        public static void ProcessStroke(Stroke s)
        {
            // Store the type name, UwU
            var typeName = s.GetType().FullName;
            if (typeName != null)
            {
                s.AddPropertyData(AssemblyInfo.Guid, typeName);
            }
        }
    }
}
