#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

#endregion

namespace NextCanvas.Interactivity.Progress
{
    public class TaskManager<T> where T : class, IProgressInteraction
    {
        private readonly T _progress;

        public TaskManager(T progress, IEnumerable<ProgressTask> tasks)
        {
            _progress = progress;
            var progressTasks = tasks.ToList();
            if (!progressTasks.Any())
                throw new ArgumentException("There are no tasks for me to do something actually useful. :'(");
            Tasks = new ObservableCollection<ProgressTask>(progressTasks);
            foreach (var task in Tasks) AttachEvents(task);
            Tasks.CollectionChanged += TasksOnCollectionChanged;
            CurrentTask = Tasks[0];
            UpdateProgress();
        }

        public ProgressTask CurrentTask { get; private set; }
        public ObservableCollection<ProgressTask> Tasks { get; }

        private void AttachEvents(ProgressTask task)
        {
            task.PropertyChanged += Task_ProgressChanged;
            task.TaskComplete += Task_TaskComplete;
        }

        private void Task_TaskComplete(object sender, EventArgs e)
        {
            var index = Tasks.IndexOf(CurrentTask) + 1;
            if (index < Tasks.Count)
            {
                CurrentTask = Tasks[index];
            }
            UpdateProgress();
        }

        private void DetachEvents(ProgressTask task)
        {
            task.PropertyChanged -= Task_ProgressChanged;
            task.TaskComplete -= Task_TaskComplete;
        }

        private void Task_ProgressChanged(object sender, EventArgs e)
        {
            UpdateProgress();
        }

        private void UpdateProgress()
        {
            if (!Tasks.Any()) return;
            var name = CurrentTask.ProgressText;
            if (name is null) return;
            _progress.Data.ProgressText = name;
            // Let's do some quick maths
            ProcessProgress();
        }

        private void ProcessProgress()
        {
            var totalProgress = Tasks.Sum(t => t.ProgressWeight);
            var completedProgress = Tasks.Sum(t => t.ProgressWeight * (t.Progress / 100));
            var percentage = completedProgress / totalProgress * 100;
            _progress.Data.Progress = percentage;
        }

        private void TasksOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
                foreach (var item in e.NewItems)
                    AttachEvents((ProgressTask)item);

            if (e.OldItems != null)
                foreach (var item in e.OldItems)
                    DetachEvents((ProgressTask)item);
            UpdateProgress();
        }

        public void WorkDone()
        {
            foreach (var task in Tasks)
            {
                DetachEvents(task);
            }
            Tasks.CollectionChanged -= TasksOnCollectionChanged;
            Tasks.Clear();
            _progress.CloseInteraction();
        }
    }
    public class TaskManager : TaskManager<IProgressInteraction>
    {
        public TaskManager(IProgressInteraction progress, IEnumerable<ProgressTask> tasks) : base(progress, tasks)
        {
        }
    }
}