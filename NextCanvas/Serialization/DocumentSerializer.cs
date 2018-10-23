
using Ionic.Zip;
using Newtonsoft.Json;
using NextCanvas.Interactivity;
using NextCanvas.Models;
using NextCanvas.Models.Content;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
        public Task SaveCompressedDocument(Document document, string savePath, IProgressInteraction progress = null)
        {
            return Task.Run(() => CreateZipFile(document, savePath, progress));
        }
        private void CreateZipFile(Document document, string savePath)
        {
            using (var zip = new ZipFile())
            {
                AddDocumentJson(document, zip);
                zip.AddDirectoryByName("resources");
                foreach (var resource in document.Resources)
                {
                    resource.Data.Position = 0;
                    zip.AddEntry($"resources\\{resource.Name}", resource.Data);
                }

                zip.Save(savePath);
            }
        }

        private void AddDocumentJson(Document document, ZipFile zip)
        {
            var mainJson = JsonConvert.SerializeObject(document, serializerSettings);
            zip.AddEntry("document.json", mainJson);
        }

        private void CreateZipFile(Document document, string savePath, IProgressInteraction progress)
        {
            using (var zip = new ZipFile())
            {
                progress.Show();
                progress.ProgressText = "Writing document data...";
                progress.Progress = 10;
                AddDocumentJson(document, zip);
                zip.AddDirectoryByName("resources");
                var resourcesCount = document.Resources.Count;
                var increment = (100 - progress.Progress) / resourcesCount;
                if (resourcesCount == 0)
                {
                    FinalizeFile(savePath, zip);
                    return;
                }
                foreach (var resource in document.Resources)
                {
                    progress.Progress += increment / 2;
                    progress.ProgressText = $"Writing resource : {resource.Name}";
                    resource.Data.Position = 0;
                    zip.AddEntry($"resources\\{resource.Name}", resource.Data);
                    progress.Progress += increment / 2;
                }
                FinalizeFile(savePath, zip);
            }
        }

        private static void FinalizeFile(string savePath, ZipFile zip)
        {
            zip.Save(savePath);
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