using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Input;

namespace Xam.Applications.GradientEditor
{

    public class RelayCommand : ICommand
    {
        #region Private vars
        readonly Action<object> execute;
        readonly Predicate<object> canExecute;
        private EventHandler _internalCanExecuteChanged;
        #endregion

        /************************************************************************/

        #region Constructors
        public RelayCommand(Action<object> execute, Predicate<object> canExecute, string description)
        {
            if (execute == null) throw new ArgumentNullException("execute");
            this.execute = execute;
            this.canExecute = canExecute;
            Description = description;
        }
        #endregion

        /************************************************************************/

        #region Public Properties
        /// <summary>
        /// Gets the description of this command
        /// </summary>
        public string Description
        {
            get;
            private set;
        }
        #endregion

        /************************************************************************/

        #region ICommand Members

        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return canExecute == null ? true : canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                _internalCanExecuteChanged += value;
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                _internalCanExecuteChanged -= value;
                CommandManager.RequerySuggested -= value;
            }
        }

        public void Execute(object parameter)
        {
            execute(parameter);
        }


        /// <summary>
        /// This method can be used to raise the CanExecuteChanged handler.
        /// This will force WPF to re-query the status of this command directly.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            if (canExecute != null)
                OnCanExecuteChanged();
        }

        /// <summary>
        /// This method is used to walk the delegate chain and well WPF that
        /// our command execution status has changed.
        /// </summary>
        protected virtual void OnCanExecuteChanged()
        {
            EventHandler eCanExecuteChanged = _internalCanExecuteChanged;
            if (eCanExecuteChanged != null)
                eCanExecuteChanged(this, EventArgs.Empty);
        }
        #endregion
    }

}
