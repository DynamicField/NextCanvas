using System;

namespace NextCanvas.Interactivity.Progress
{
    public class ProgressTask : PropertyChangedObject, IProgressData
    {
        private double progress;

        public event EventHandler TaskComplete;
        private string progressText;

        public ProgressTask(double weight = 50, string text = "Something goes there...")
        {
            ProgressWeight = weight;
            ProgressText = text;
        }

        // The weight in the total manager.
        // Like : 10, 50, 40 = 100 
        public double ProgressWeight { get; }

        public double Progress
        {
            get => IsComplete ? 100 : progress;
            set
            {
                if (IsComplete) return;
                progress = Math.Min(100,value);
                if (progress >= 99.999)
                {
                    OnPropertyChanged();
                    Complete();                   
                }
                else
                {
                    OnPropertyChanged();
                }
            }
        }
        public string ProgressText
        {
            get => progressText;
            set
            {
                if (IsComplete) return;
                progressText = value;
                OnPropertyChanged();
            }
        }

        public bool IsComplete { get; private set; }

        public void Complete()
        {
            IsComplete = true;
            Progress = 100;
            OnPropertyChanged(nameof(Progress));
            TaskComplete?.Invoke(this, EventArgs.Empty);
        }
    }
}