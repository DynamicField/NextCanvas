#region

using System;
using System.IO;
using System.Linq;
using Ionic.Zip;
using NextCanvas.Interactivity.Progress;
using NextCanvas.Models;
using NextCanvas.Models.Content;

#endregion

namespace NextCanvas.Serialization
{
    public class DocumentReader : DocumentSerializerBase
    {
        // TODO: Implement smart zip updating.

        public Document TryOpenDocument(FileStream fileStream, IProgressInteraction interaction)
        {
            try
            {
                return OpenCompressedFileFormat(fileStream, interaction);
            }
            catch (ZipException) // Try reading as json
            {
                return OpenJson(fileStream);
            }
        }

        public Document OpenCompressedFileFormat(Stream fileStream, IProgressInteraction interaction)
        {
            using (var zipFile = ZipFile.Read(fileStream))
            {
                var doc = GetDocumentJson(zipFile);
                foreach (var resource in doc.Resources)
                    ProcessDataCopying(zipFile, resource); // Copy the deeta to the resources.
                return doc; // Yeah we're done :) dope nah?
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
    }
}