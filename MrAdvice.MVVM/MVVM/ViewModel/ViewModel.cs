
namespace ArxOne.MrAdvice.MVVM.ViewModel
{
    using System.ComponentModel;
    using System.Reflection;
    using Properties;
    using Threading;

    /// <summary>
    /// View-model base
    /// </summary>
    public class ViewModel : ILoadViewModel, INotifyPropertyChangedViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [UISync]
        public virtual void OnPropertyChanged(PropertyInfo propertyInfo, NotifyPropertyChanged sender)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyInfo.Name));
        }

        /// <summary>
        /// Raises the <see cref="E:PropertyChanged" /> event.
        /// </summary>
        /// <param name="args">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            var onPropertyChanged = PropertyChanged;
            if (onPropertyChanged != null)
                onPropertyChanged(this, args);
        }

        /// <summary>
        /// Loads data related to this view-model.
        /// </summary>
        public virtual void Load()
        { }
    }
}
