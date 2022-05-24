using System;
using System.Windows.Input;

namespace CourseProject.ViewModel
{
        public class DelegateCommand : ICommand
        {
            private readonly Action<object> execute;
            private readonly Func<object, bool> canExecute;

            public DelegateCommand(Action<object> execute)
                : this(execute, null)
            { }

            public DelegateCommand(Action<object> executeAction, Func<object, bool> canExecuteAction)
            {
                execute = executeAction;
                canExecute = canExecuteAction;
            }

            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }

            public bool CanExecute(object parameter)
            {
                return canExecute == null ? true : canExecute(parameter);
            }

            public void Execute(object parameter)
            {
                if (execute != null)
                    execute(parameter);
            }
        }
}
