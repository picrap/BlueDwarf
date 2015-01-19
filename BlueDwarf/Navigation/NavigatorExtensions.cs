// This is the blue dwarf
// more information at https://code.google.com/p/blue-dwarf/

namespace BlueDwarf.Navigation
{
    using System;
    using System.Windows;
    using ViewModel;

    public static class NavigatorExtensions
    {
        public static void Configure<TViewModel, TView>(this INavigator navigator)
            where TViewModel : ViewModel
            where TView : FrameworkElement
        {
            navigator.Configure(typeof(TViewModel), typeof(TView));
        }

        public static TViewModel Show<TViewModel>(this INavigator navigator, Action<TViewModel> initializer = null)
            where TViewModel : ViewModel
        {
            var objectInitializer = initializer != null ? delegate(object o) { initializer((TViewModel)o); } : (Action<object>)null;
            return (TViewModel)navigator.Show(typeof(TViewModel), objectInitializer);
        }
    }
}
