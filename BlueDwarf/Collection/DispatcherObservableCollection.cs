// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf

namespace BlueDwarf.Collection
{
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using ArxOne.MrAdvice.MVVM.Threading;
    using Utility;

    public class DispatcherObservableCollection<TItem> : ObservableCollection<TItem>
    {
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            BaseOnCollectionChanged(e);
        }

        [UISync]
        private void BaseOnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnCollectionChanged(e);
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            BaseOnPropertyChanged(e);
        }

        [UISync]
        private void BaseOnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
        }
    }
}
