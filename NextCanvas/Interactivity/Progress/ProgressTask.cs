#region

using System;

#endregion

namespace NextCanvas.Interactivity.Progress
{
    public class ProgressTask : PropertyChangedObject, IProgressData
    {
        private double _progress;
        private string _progressText;

        public ProgressTask(double weight = 50, string text = "Something goes there...")
        {
            ProgressWeight = weight;
            ProgressText = text;
        }

        // The weight in the total manager.
        // Like : 10, 50, 40 = 100 
        public double ProgressWeight { get; }

        public bool IsComplete { get; private set; }

        public double Progress
        {
            get => IsComplete ? 100 : _progress;
            set
            {
                if (IsComplete) return;
                _progress = Math.Min(100, value);
                if (_progress >= 99.999)
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
            get => _progressText;
            set
            {
                if (IsComplete) return;
                _progressText = value;
                OnPropertyChanged();
            }
        }

        public event EventHandler TaskComplete;

        public void Complete()
        {
            IsComplete = true;
            Progress = 100;
            OnPropertyChanged(nameof(Progress));
            TaskComplete?.Invoke(this, EventArgs.Empty);
        }
    }
}