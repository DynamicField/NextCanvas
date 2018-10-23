using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace NextCanvas.Interactivity.Progress
{
    public class TaskManager<T> where T : class, IProgressInteraction
    {
        private readonly T progress;
        public ProgressTask CurrentTask { get; private set; }

        public ObservableCollection<ProgressTask> Tasks { get; }

        public TaskManager(T progress, IEnumerable<ProgressTask> tasks)
        {
            this.progress = progress;
            List<ProgressTask> progressTasks = tasks.ToList();
            if (!progressTasks.Any())
            {
                throw new ArgumentException("There are no tasks for me to do something actually useful. :'(");
            }
            Tasks = new ObservableCollection<ProgressTask>(progressTasks);
            foreach (ProgressTask task in Tasks)
            {
                AttachEvents(task);
            }
            Tasks.CollectionChanged += TasksOnCollectionChanged;
            CurrentTask = Tasks[0];
            UpdateProgress();
        }

        private void AttachEvents(ProgressTask task)
        {
            task.ProgressChanged += Task_ProgressChanged;
            task.TaskComplete += Task_TaskComplete;
        }

        private void Task_TaskComplete(object sender, EventArgs e)
        {
            int index = Tasks.IndexOf(CurrentTask) + 1;
            if (index == Tasks.Count)
            {
                progress.Close();
                return;
            }
            CurrentTask = Tasks[index];
            UpdateProgress();
        }

        private void DetachEvents(ProgressTask task)
        {
            task.ProgressChanged -= Task_ProgressChanged;
            task.TaskComplete -= Task_TaskComplete;
        }

        private void Task_ProgressChanged(object sender, EventArgs e)
        {
            UpdateProgress();
        }

        private void UpdateProgress()
        {
            string name = Tasks.First(t => !t.IsComplete)
                               .ProgressText;
            progress.Data.ProgressText = name;
            // Let's do some quick maths
            ProcessProgress();
        }

        private void ProcessProgress()
        {
            double totalProgress = Tasks.Sum(t => t.ProgressWeight);
            double completedProgress = Tasks.Sum(t => t.ProgressWeight * (t.Progress / 100));
            double percentage = completedProgress / totalProgress * 100;
            progress.Data.Progress = percentage;
        }

        private void TasksOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (object item in e.NewItems)
                {
                    AttachEvents((ProgressTask) item);
                }
            }

            if (e.OldItems != null)
            {
                foreach (object item in e.OldItems)
                {
                    DetachEvents((ProgressTask) item);
                }
            }
        }
    }
}