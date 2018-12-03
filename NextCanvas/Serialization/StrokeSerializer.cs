using System;
using System.Collections.Generic;
using System.IO;
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

        public static StrokeCollection DeserializeStrokes(byte[] strokes)
        {
            using (var memory = new MemoryStream(strokes))
            {
                var strokeCollection = new StrokeCollection(memory);
                var toRemove = new StrokeCollection();
                var toAdd = new StrokeCollection();
                foreach (var stroke in strokeCollection)
                {
                    if (!stroke.ContainsPropertyData(AssemblyInfo.Guid)) continue;
                    try
                    {
                        var type = Type.GetType((string)stroke.GetPropertyData(AssemblyInfo.Guid));
                        if (type != null)
                        {
                            var customStroke = (Stroke)Activator.CreateInstance(type, stroke.StylusPoints,
                                stroke.DrawingAttributes);
                            toRemove.Add(stroke);
                            toAdd.Add(customStroke);
                        }
                    }
                    catch (Exception)
                    {
                        // whatever
                    }
                }

                strokeCollection.Remove(toRemove);
                strokeCollection.Add(toAdd);

                return strokeCollection;
            }
        }

        public static byte[] StrokesToBytes(StrokeCollection strokes)
        {
            using (var memory = new MemoryStream())
            {
                strokes.Save(memory);
                return memory.ToArray();
            }
        }
    }
}
