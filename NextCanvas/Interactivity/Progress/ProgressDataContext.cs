namespace NextCanvas.Interactivity.Progress
{
    public class ProgressDataContext : PropertyChangedObject, IProgressData
    {
        private double _progress;
        private string _progressText;

        public double Progress
        {
            get => _progress;
            set
            {
                _progress = value;
                OnPropertyChanged(nameof(Progress));
            }
        }

        public string ProgressText
        {
            get => _progressText;
            set
            {
                _progressText = value;
                OnPropertyChanged(nameof(ProgressText));
            }
        }
    }
}
