using System;

namespace NextCanvas.Interactivity.Progress
{
    public class ProgressTask : IProgressData
    {
        private double progress;
        private string progressText;

        // The weight in the total manager.
        // Like : 10, 50, 40 = 100 
        public double ProgressWeight { get; }

        public bool IsComplete { get; private set; }

        public ProgressTask(double weight = 50, string text = "Something goes there...")
        {
            ProgressWeight = weight;
            ProgressText = text;
        }

        public double Progress
        {
            get => progress;
            set
            {
                if (IsComplete)
                {
                    return;
                }
                progress = Math.Min(100, value);
                if (progress >= 99.999)
                {
                    Complete();
                }
                else
                {
                    ProgressChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public string ProgressText
        {
            get => progressText;
            set
            {
                if (IsComplete)
                {
                    return;
                }
                progressText = value;
                ProgressChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler ProgressChanged;
        public event EventHandler TaskComplete;

        public void Complete()
        {
            IsComplete = true;
            Progress = 100;
            TaskComplete?.Invoke(this, EventArgs.Empty);
        }
    }
}