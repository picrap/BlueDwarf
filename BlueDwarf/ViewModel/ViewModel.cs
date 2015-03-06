// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.ViewModel
{
    using System;
    using System.Reflection;
    using ArxOne.MrAdvice.MVVM.Properties;
    using ArxOne.MrAdvice.MVVM.Threading;
    using Properties;

    /// <summary>
    /// View-model base (in case we write more than one).
    /// </summary>
    public class ViewModel : ArxOne.MrAdvice.MVVM.ViewModel.ViewModel
    {
        [NotifyPropertyChanged]
        public bool Loading { get; set; }

        [UISync]
        public override void OnPropertyChanged(PropertyInfo propertyInfo, NotifyPropertyChanged sender)
        {
            var categoryNotifyPropertyChanged = sender as CategoryNotifyPropertyChanged;
            var category = categoryNotifyPropertyChanged != null ? categoryNotifyPropertyChanged.Category : null;
            OnPropertyChanged(new CategoryPropertyChangedEventArgs(propertyInfo.Name, category));
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
