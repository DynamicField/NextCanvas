using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Ink;
using Newtonsoft.Json;
using NextCanvas.Models.Content;
using NextCanvas.Properties;

namespace NextCanvas.Models
{
    public class Page
    {
        [JsonIgnore] public StrokeCollection Strokes { get; set; } = new StrokeCollection();

        public byte[] StrokesSerialized
        {
            get
            {
                using (var memory = new MemoryStream())
                {
                    Strokes.Save(memory);
                    return memory.ToArray();
                }
            }
            set
            {
                using (var memory = new MemoryStream(value))
                {
                    var strokeCollection = new StrokeCollection(memory);
                    var toRemove = new StrokeCollection();
                    var toAdd = new StrokeCollection();
                    foreach (var stroke in strokeCollection)
                    {
                        if (!stroke.ContainsPropertyData(AssemblyInfo.Guid)) continue;
                        try
                        {
                            var type = Type.GetType((string) stroke.GetPropertyData(AssemblyInfo.Guid));
                            var customStroke = (Stroke) Activator.CreateInstance(type, stroke.StylusPoints,
                                stroke.DrawingAttributes);
                            toRemove.Add(stroke);
                            toAdd.Add(customStroke);
                        }
                        catch (Exception)
                        {
                            // whatever
                        }
                    }

                    strokeCollection.Remove(toRemove);
                    strokeCollection.Add(toAdd);

                    Strokes = strokeCollection;
                }
            }
        }

        public List<ContentElement> Elements { get; set; } = new List<ContentElement>();
        public int Width { get; set; } = 1720;
        public int Height { get; set; } = 1420;
    }
}