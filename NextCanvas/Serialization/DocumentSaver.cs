using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Ionic.Zip;
using Ionic.Zlib;
using Newtonsoft.Json;
using NextCanvas.Interactivity.Progress;
using NextCanvas.Models;

namespace NextCanvas.Serialization
{
    public class DocumentSaver : DocumentSerializerBase
    {
        public Task SaveCompressedDocument(Document document, string savePath, IProgressInteraction progress)
        {
            return Task.Run(() => CreateZipFile(document, savePath, progress));
        }

        private void AddDocumentJson(Document document, ZipFile zip)
        {
            var mainJson = JsonConvert.SerializeObject(document, SerializerSettings);
            zip.AddEntry("document.json", mainJson, Encoding.UTF8); // Fixes the things
        }

        private void CreateZipFile(Document document, string savePath, IProgressInteraction progress)
        {
            using (var zip = new ZipFile
                {CompressionLevel = (CompressionLevel) SettingsManager.Settings.FileCompressionLevel})
            {
                var taskManager = CreateSavingTasksInitialization(document, progress, out var writingTask,
                    out var resourceTasks, out var count, out var finalizingTask);
                progress.ShowInteraction();
                InitializeZipStructure(document, zip, writingTask);
                if (count == 0)
                {
                    FinalizeFileTask(savePath, finalizingTask, zip);
                    taskManager.WorkDone();
                    return;
                }
                ProcessResources(document, resourceTasks, zip);
                FinalizeFileTask(savePath, finalizingTask, zip);
                taskManager.WorkDone();
            }
        }

        private void InitializeZipStructure(Document document, ZipFile zip, ProgressTask writingTask)
        {
            AddDocumentJson(document, zip);
            zip.AddDirectoryByName("resources");
            writingTask.Complete();
        }

        private static void ProcessResources(Document document, List<ProgressTask> resourceTasks, ZipFile zip)
        {
            for (var index = 0; index < document.Resources.Count; index++)
            {
                var task = resourceTasks[index];
                var resource = document.Resources[index];
                task.Progress = 50; // yeah ok
                resource.Data.Position = 0;
                zip.AddEntry($"resources\\{resource.Name}", resource.Data); // add the resource deeta.
                resourceTasks[index].Complete();
            }
        }

        private static TaskManager<IProgressInteraction> CreateSavingTasksInitialization(Document document,
            IProgressInteraction progress, out ProgressTask writingTask,
            out List<ProgressTask> resourceTasks, out int count, out ProgressTask finalizingTask)
        {
            writingTask = new ProgressTask(10, "Writing document base data...");
            var tasks = new List<ProgressTask>
            {
                writingTask
            };
            resourceTasks = new List<ProgressTask>();
            count = document.Resources.Count;
            for (var i = 0; i < count; i++)
            {
                var progressTask = new ProgressTask(10, $"Writing resource : {document.Resources[i].Name}...");
                tasks.Add(progressTask);
                resourceTasks.Add(progressTask);
            }

            finalizingTask = new ProgressTask(5, "Saving to file...");
            tasks.Add(finalizingTask);
            return new TaskManager<IProgressInteraction>(progress, tasks);
        }

        private static void FinalizeFileTask(string savePath, ProgressTask finalizingTask, ZipFile zip)
        {
            finalizingTask.Progress = 50;
            FinalizeFile(savePath, zip);
            finalizingTask.Complete();
        }

        private static void FinalizeFile(string savePath, ZipFile zip)
        {
            zip.Save(savePath);
        }
    }
}