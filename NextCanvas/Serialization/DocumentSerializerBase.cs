using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NextCanvas.Models;
using NextCanvas.Models.Content;

namespace NextCanvas.Serialization
{
    public abstract class DocumentSerializerBase
    {
        protected readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            Error = IgnoreError
        };

        private static void IgnoreError(object sender, ErrorEventArgs e)
        {
            if (e.CurrentObject is Page p &&
                Regex.IsMatch(e.ErrorContext.Path, @"Pages\.\$values\[[0-9]+\]\.Elements\.\$type")) // If type is wrong
                p.Elements.Add(new ContentElement()); // oh no
            e.ErrorContext.Handled = true;
        }
    }
}