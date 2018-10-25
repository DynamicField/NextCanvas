using System;
using System.IO;
using System.Linq;
using Ionic.Zip;
using Newtonsoft.Json;
using NextCanvas.Models;
using NextCanvas.Models.Content;

namespace NextCanvas.Serialization
{
    public class DocumentReader : DocumentSerializerBase
    {
        // TODO: Implement smart zip updating.

        public Document TryOpenDocument(FileStream fileStream)
        {
            try
            {
                return OpenCompressedFileFormat(fileStream);
            }
            catch (ZipException) // Try reading as json
            {
                return OpenJson(fileStream);
            }
        }

        public Document OpenCompressedFileFormat(Stream fileStream)
        {
            using (var zipFile = ZipFile.Read(fileStream))
            {
                var doc = GetDocumentJson(zipFile);
                foreach (var resource in doc.Resources)
                    ProcessDataCopying(zipFile, resource); // Copy the deeta to the resources.
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
            var deserialized = JsonConvert.DeserializeObject<Document>(value, SerializerSettings);
            return deserialized;
        }
    }
}