using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Input;

namespace BlueDwarf.ViewModel
{
    /// <summary>
    /// View-model base (in case we write more than one).
    /// </summary>
    public class ViewModel : INotifyPropertyChanged, ICommand
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when property changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        public void OnPropertyChanged(string propertyName)
        {
            var onPropertyChanged = PropertyChanged;
            if (onPropertyChanged != null)
                onPropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        // TODO
        public event EventHandler CanExecuteChanged;

        // TODO
        public bool CanExecute(object parameter)
        {
            return true;
        }

        private static readonly object[] NoParameter = new object[0];

        public void Execute(object parameter)
        {
            var methodBase = parameter as MethodBase;
            if (methodBase != null)
                methodBase.Invoke(this, NoParameter);
            else
                throw new InvalidOperationException("Can not handle simple messages now (and maybe never)");
        }
    }
}
