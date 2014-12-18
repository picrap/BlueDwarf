using System.Linq;
using System.Windows;
using BlueDwarf.Utility;

namespace BlueDwarf.Controls
{
    public class Hider : FrameworkElement
    {
        public static readonly DependencyProperty HideProperty = DependencyProperty.Register(
            "Hide", typeof(bool), typeof(Hider), new PropertyMetadata(default(bool), OnHideChanged));

        public bool Hide
        {
            get { return (bool)GetValue(HideProperty); }
            set { SetValue(HideProperty, value); }
        }

        private static void OnHideChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var @this = (Hider)d;
            @this.ShowHide();
        }

        private void ShowHide()
        {
            var parentWindow = this.GetSelfAndParents().OfType<Window>().FirstOrDefault();
            if (parentWindow == null)
                return;

            parentWindow.Visibility = Hide ? Visibility.Hidden : Visibility.Visible;
        }
    }
}
