﻿using System;
using System.Windows.Input;

namespace NextCanvas
{
    public class DelegateCommand : ICommand
    {
        public DelegateCommand(Action<object> action, Func<object, bool> canExecute = null)
        {
            ActionToExecute = action;
            if (canExecute != null) CanExecuteAction = canExecute;
        }

        public DelegateCommand(Action act, Func<object, bool> canExecute = null) : this(_ => act(), canExecute) { }
        public DelegateCommand(Action act, Func<bool> canExecute) : this(act, _ => canExecute()) { }
        public Action<object> ActionToExecute { get; set; }
        public Func<object, bool> CanExecuteAction { get; set; } = o => true;
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return CanExecuteAction(parameter);
        }

        public void Execute(object parameter)
        {
            ActionToExecute(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }
}