// This is the blue dwarf
// more information at https://code.google.com/p/blue-dwarf/

namespace BlueDwarf.Controls
{
    using System.Linq;
    using System.Windows;
    using Utility;
    using ViewModel.Properties;

    public class Hider : FrameworkElement
    {
        [View.Properties.DependencyProperty(Notification = DependencyPropertyNotification.OnPropertyNameChanged)]
        public bool Show { get; set; }

        /// <summary>
        /// Called when Show property changed.
        /// Raised automatically
        /// </summary>
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
