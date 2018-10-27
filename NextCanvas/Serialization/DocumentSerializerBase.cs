using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Ionic.Zip;
using Newtonsoft.Json;
using NextCanvas.Models;
using NextCanvas.Models.Content;
using ErrorEventArgs = Newtonsoft.Json.Serialization.ErrorEventArgs;

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

        protected Document GetDocumentJson(ZipFile zipFile)
        {
            var documentJson = zipFile.Entries.First(e => e.FileName == "document.json");
            Document doc;
            using (var docReader = documentJson.OpenReader())
            {
                doc = GetBaseDocumentJson(docReader);
            }

            return doc;
        }

        protected Document GetBaseDocumentJson(Stream docReader)
        {
            Document doc;
            using (var streamReader = new StreamReader(docReader))
            {
                doc = ReadDocumentJson(streamReader);
            }

            return doc;
        }

        protected Document ReadDocumentJson(TextReader streamyStream)
        {
            var value = streamyStream.ReadToEnd();
            var deserialized = JsonConvert.DeserializeObject<Document>(value, SerializerSettings);
            return deserialized;
        }
    }
}