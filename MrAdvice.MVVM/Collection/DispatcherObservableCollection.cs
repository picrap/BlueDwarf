
namespace ArxOne.MrAdvice.Collection
{
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using MVVM.Threading;

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
