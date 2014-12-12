using System.ComponentModel;
using BlueDwarf.Annotations;

namespace BlueDwarf.ViewModel
{
    public class ViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        public virtual void OnPropertyChanged(string propertyName)
        {
            var onPropertyChanged = PropertyChanged;
            if (onPropertyChanged != null) 
                onPropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
