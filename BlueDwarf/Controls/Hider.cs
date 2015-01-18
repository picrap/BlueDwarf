using System.Linq;
using System.Windows;
using BlueDwarf.Aspects;
using BlueDwarf.Utility;

namespace BlueDwarf.Controls
{
    public class Hider : FrameworkElement
    {
        [Aspects.DependencyProperty(Notification = DependencyPropertyNotification.OnPropertyNameChanged)]
        public bool Show { get; set; }

        public void OnShowChanged()
        {
            ShowHide();
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
            var parentWindow = this.GetLogicalSelfAndParents().OfType<Window>().FirstOrDefault();
            if (parentWindow == null)
                return;

            parentWindow.Visibility = Show ? Visibility.Visible : Visibility.Hidden;
        }
    }
}
