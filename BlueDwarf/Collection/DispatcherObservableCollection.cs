
namespace BlueDwarf.Collection
{
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using Utility;

    public class DispatcherObservableCollection<TItem> : ObservableCollection<TItem>
    {
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            BaseOnCollectionChanged(e);
        }

        //[UISync]
        private void BaseOnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            UISync.Invoke(() => base.OnCollectionChanged(e));
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            BaseOnPropertyChanged(e);
        }

        //[UISync]
        private void BaseOnPropertyChanged(PropertyChangedEventArgs e)
        {
            UISync.Invoke(() => base.OnPropertyChanged(e));
        }
    }
}
