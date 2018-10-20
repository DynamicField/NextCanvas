using System;
using System.Windows.Input;

namespace NextCanvas
{
    public class DelegateCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public Action<object> ActionToExecute { get; set; }
        public Func<object, bool> CanExecuteAction { get; set; } = o => true;
        public bool CanExecute(object parameter)
        {
            return CanExecuteAction(parameter);
        }

        public void Execute(object parameter)
        {
            ActionToExecute(parameter);
        }
        public DelegateCommand(Action<object> action, Func<object, bool> canExecute = null)
        {
            ActionToExecute = action;
            if (canExecute != null)
            {
                CanExecuteAction = canExecute;
            }
        }
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }
}
