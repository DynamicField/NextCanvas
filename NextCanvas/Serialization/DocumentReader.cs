#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Ionic.Zip;
using NextCanvas.Interactivity.Progress;
using NextCanvas;
using NextCanvas.Content;

#endregion

namespace NextCanvas.Serialization
{
    public class DocumentReader : DocumentSerializerBase
    {
        // TODO: Implement smart zip updating.

        public async Task<Document> TryOpenDocument(FileStream fileStream, IProgressInteraction interaction)
        {
            try
            {
                return await Task.Run(() => OpenCompressedFileFormat(fileStream, interaction));
            }
            catch (ZipException) // Try reading as json
            {
                return OpenJson(fileStream);
            }
        }

        public Document OpenCompressedFileFormat(Stream fileStream, IProgressInteraction interaction)
        {
            var taskManager = new TaskManager(interaction, new []
            {
                new ProgressTask(10, "Reading document base data...")
            });
            interaction.ShowInteraction();
            using (var zipFile = ZipFile.Read(fileStream))
            {
                var doc = GetDocumentJson(zipFile);
                taskManager.Tasks[0].Complete();
                var resourceTasks = doc.Resources.Select(r => new ProgressTask(40,
                    $"Reading resource {r.Name} ({doc.Resources.IndexOf(r) + 1}/{doc.Resources.Count})"));
                var progressTasks = resourceTasks as ProgressTask[] ?? resourceTasks.ToArray();
                foreach (var task in progressTasks)
                {
                    taskManager.Tasks.Add(task);
                }
                foreach (var resource in doc.Resources)
                    ProcessDataCopying(zipFile, resource, progressTasks[doc.Resources.IndexOf(resource)]); // Copy the deeta to the resources.
                taskManager.WorkDone();
                return doc; // Yeah we're done :) dope nah?
            }
        }

        private static void ProcessDataCopying(ZipFile zipFile, Resource resource, ProgressTask task = null)
        {
            var data = zipFile.Entries.First(e =>
                e.FileName.Equals($"resources/{resource.Name}", StringComparison.InvariantCultureIgnoreCase));
            var stream = new MemoryStream();
            data.Extract(stream);
            resource.Data = stream;
            task?.Complete();
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