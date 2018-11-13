using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextCanvas.Models;

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
