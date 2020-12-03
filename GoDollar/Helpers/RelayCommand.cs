using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace GoDollar.Helpers
{
    public class RelayCommand<T>  : ICommand 
    {
        private readonly Action action;
        private readonly Action<T> paramAction;

        private readonly Predicate<T> canExecute;      


        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }


        public RelayCommand(Action action)
        {
            this.action = action;
            canExecute = null;
        }

        public RelayCommand(Action<T> action)
        {            
            paramAction = action;
            canExecute = null;            
        }
        public RelayCommand(Action action, Predicate<T> canExecute)
        {
            this.action = action;
            this.canExecute = canExecute;
        }
        public RelayCommand(Action<T> action, Predicate<T> canExecute )
        {           
            paramAction = action;
            this.canExecute = canExecute;        
        }


        public bool CanExecute(object parameter)
        {
            return canExecute != null ? canExecute((T)parameter) : true;
        }

        public void Execute(object parameter)
        {
            if (paramAction != null)
                paramAction((T)parameter);
            else
                action();
        }
    }
}
