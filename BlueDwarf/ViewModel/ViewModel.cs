using System.ComponentModel;

namespace BlueDwarf.ViewModel
{
    /// <summary>
    /// View-model base (in case we write more than one).
    /// </summary>
    public class ViewModel: INotifyPropertyChanged
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
    }
}
