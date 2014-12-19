using System.Linq;
using System.Windows;
using BlueDwarf.Utility;

namespace BlueDwarf.Controls
{
    public class Hider : FrameworkElement
    {
        public static readonly DependencyProperty ShowProperty = DependencyProperty.Register(
            "Show", typeof(bool), typeof(Hider), new PropertyMetadata(default(bool), OnShowChanged));

        public bool Show
        {
            get { return (bool)GetValue(ShowProperty); }
            set { SetValue(ShowProperty, value); }
        }

        private static void OnShowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var @this = (Hider)d;
            @this.ShowHide();
        }

        public Hider()
        {
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ShowHide();
        }

        private void ShowHide()
        {
            var parentWindow = this.GetSelfAndParents().OfType<Window>().FirstOrDefault();
            if (parentWindow == null)
                return;

            parentWindow.Visibility = Show ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
