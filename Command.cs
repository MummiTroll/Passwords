using System;
using System.Windows.Input;

namespace Passwords
{
    public class Command : ICommand
    {
        private Action<object> _action;
        private Func<bool> _canExecute;
        public Command(Action<object> action, Func<bool> canExecute)
        {
            _action = action; _canExecute = canExecute;
        }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public bool CanExecute(object parameter)
        {
            return _canExecute.Invoke();
        }
        public void Execute(object parameter)
        {
            _action(parameter);
        }
    }
}
