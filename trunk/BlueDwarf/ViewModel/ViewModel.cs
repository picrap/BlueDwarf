// This is the blue dwarf
// more information at https://code.google.com/p/blue-dwarf/

namespace BlueDwarf.ViewModel
{
    using System;
    using System.ComponentModel;
    using System.Reflection;
    using System.Windows.Input;
    using Aspects;

    /// <summary>
    /// View-model base (in case we write more than one).
    /// </summary>
    public class ViewModel : INotifyPropertyChanged, ICommand
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChanged]
        public bool Loading { get; set; }

        /// <summary>
        /// Called when property changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="category">The category.</param>
        public void OnPropertyChanged(string propertyName, object category = null)
        {
            var onPropertyChanged = PropertyChanged;
            if (onPropertyChanged != null)
                onPropertyChanged(this, new CategoryPropertyChangedEventArgs(propertyName, category));
        }

        /// <summary>
        /// Loads data related to this view-model.
        /// </summary>
        public virtual void Load()
        { }

        // TODO
        public event EventHandler CanExecuteChanged;

        // TODO
        public bool CanExecute(object parameter)
        {
            return true;
        }

        private static readonly object[] NoParameter = new object[0];

        /// <summary>
        /// Invokes the command.
        /// </summary>
        /// <param name="parameter">Optional parameter.</param>
        public void Execute(object parameter)
        {
            var methodBase = parameter as MethodBase;
            if (methodBase != null)
                methodBase.Invoke(this, NoParameter);
            else
                throw new InvalidOperationException("Can not handle simple messages now (and maybe never)");
        }

        /// <summary>
        /// Asynchronously invokes the action and shows the wait overlay (assuming view handles it).
        /// </summary>
        /// <param name="action">The action.</param>
        [Async]
        protected void ShowAsyncLoad(Action action)
        {
            bool loading = Loading;
            try
            {
                Loading = true;
                action();
            }
            finally
            {
                Loading = loading;
            }
        }
    }
}
