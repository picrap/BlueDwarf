
using System.Runtime.InteropServices;
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

        public static TViewModel Show<TViewModel>(this INavigator navigator)
            where TViewModel : ViewModel.ViewModel
        {
            return (TViewModel)navigator.Show(typeof(TViewModel));
        }
    }
}
