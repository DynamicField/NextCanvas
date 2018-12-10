using System.Text;
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
            Model.SetBuilder(_logStringBuilder);
        }
        private StringBuilder _logStringBuilder = new StringBuilder();

        public void Append(string s)
        {
            _logStringBuilder.AppendLine(s);
        }
        public string LogString
        {
            get => _logStringBuilder.ToString();
            set
            {
                _logStringBuilder = new StringBuilder(value);
                Model.SetBuilder(_logStringBuilder);
                OnPropertyChanged(nameof(LogString));
            }
        }
    }
}
