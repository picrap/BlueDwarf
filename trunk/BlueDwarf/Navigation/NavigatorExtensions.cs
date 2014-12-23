
using System;
using System.Windows;

namespace BlueDwarf.Navigation
{
    public static class NavigatorExtensions
    {
        public static void Configure<TViewModel, TView>(this INavigator navigator)
            where TViewModel : ViewModel.ViewModel
            where TView : FrameworkElement
        {
            navigator.Configure(typeof(TViewModel), typeof(TView));
        }

        public static TViewModel Show<TViewModel>(this INavigator navigator, Action<TViewModel> initializer = null)
            where TViewModel : ViewModel.ViewModel
        {
            var objectInitializer = initializer != null ? delegate(object o) { initializer((TViewModel)o); } : (Action<object>)null;
            return (TViewModel)navigator.Show(typeof(TViewModel), objectInitializer);
        }
    }
}
