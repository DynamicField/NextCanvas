using NextCanvas;

namespace NextCanvas.ViewModels
{
    public class LogViewModel : ViewModelBase<LogModel>
    {
        public LogViewModel() : this(null)
        {
        }

        public LogViewModel(LogModel model = null) : base(model)
        {
        }

        public string LogString
        {
            get => Model.Log;
            set
            {
                Model.Log = value;
                OnPropertyChanged(nameof(LogString));
            }
        }
    }
}
