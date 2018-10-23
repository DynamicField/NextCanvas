
using Ionic.Zip;
using Newtonsoft.Json;
using NextCanvas.Models;
using NextCanvas.Models.Content;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using ErrorEventArgs = Newtonsoft.Json.Serialization.ErrorEventArgs;

namespace NextCanvas.Serialization
{
    public class DocumentSerializer
    {
        private readonly JsonSerializerSettings serializerSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            Error = IgnoreError
        };
        private static void IgnoreError(object sender, ErrorEventArgs e)
        {
            if (e.CurrentObject is Page p && Regex.IsMatch(e.ErrorContext.Path, @"Pages\.\$values\[[0-9]+\]\.Elements\.\$type")) // If type is wrong
            {
                p.Elements.Add(new ContentElement()); // oh no
            }
            e.ErrorContext.Handled = true;
        }
        // TODO: Implement smart zip updating.
        public void SaveCompressedDocument(Document document, string savePath)
        {
            CreateZipFile(document, savePath);
        }

        private void CreateZipFile(Document document, string savePath)
        {
            using (var zip = new ZipFile())
            {
                var mainJson = JsonConvert.SerializeObject(document, serializerSettings);
                zip.AddEntry("document.json", mainJson);
                zip.AddDirectoryByName("resources");
                foreach (var resource in document.Resources)
                {
                    resource.Data.Position = 0;
                    zip.AddEntry($"resources\\{resource.Name}", resource.Data);
                }

                zip.Save(savePath);
            }
        }

        public Document OpenCompressedFileFormat(Stream fileStream)
        {
            using (var zipFile = ZipFile.Read(fileStream))
            {
                var doc = GetDocumentJson(zipFile);
                foreach (var resource in doc.Resources)
                {
                    ProcessDataCopying(zipFile, resource); // Copy the deeta to the resources.
                }
                // AttachResources(doc); // Attach them to all the elements.
                return doc; // Yeah we're done :) dope nah?
            }
        }

        private Document GetDocumentJson(ZipFile zipFile)
        {
            var documentJson = zipFile.Entries.First(e => e.FileName == "document.json");
            Document doc;
            using (var docReader = documentJson.OpenReader())
            {
                doc = GetBaseDocumentJson(docReader);
            }

            return doc;
        }
        private Document GetBaseDocumentJson(Stream docReader)
        {
            Document doc;
            using (var streamReader = new StreamReader(docReader))
            {
                doc = ReadDocumentJson(streamReader);
            }
            return doc;
        }

        private static void AttachResources(Document doc)
        {
            foreach (var list in doc.Pages.Select(p => p.Elements))
            {
                foreach (var thing in list)
                {
                    if (thing is ResourceElement element && element.Resource != null)
                    {
                        element.Resource = doc.Resources.First(r => r.Name.Equals(element.Resource.Name));
                    }
                }
            }
        }

        private static void ProcessDataCopying(ZipFile zipFile, Resource resource)
        {
            var data = zipFile.Entries.First(e =>
                e.FileName.Equals($"resources/{resource.Name}", StringComparison.InvariantCultureIgnoreCase));
            var stream = new MemoryStream();
            data.Extract(stream);
            resource.Data = stream;
        }

        public Document OpenJson(Stream fileStream)
        {
            using (var streamyStream = new StreamReader(fileStream))
            {
                return ReadDocumentJson(streamyStream);
            }
        }

        private Document ReadDocumentJson(TextReader streamyStream)
        {
            var value = streamyStream.ReadToEnd();
            var deserialized = JsonConvert.DeserializeObject<Document>(value, serializerSettings);
            return deserialized;
        }
    }
}