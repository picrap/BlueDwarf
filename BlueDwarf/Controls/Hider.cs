// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Controls
{
    using System.Linq;
    using System.Windows;
    using ArxOne.MrAdvice.MVVM.Properties;
    using ArxOne.MrAdvice.Utility;
    using Utility;
    using DependencyProperty = ArxOne.MrAdvice.MVVM.Properties.DependencyProperty;

    public class Hider : FrameworkElement
    {
        [DependencyProperty(Notification = DependencyPropertyNotification.OnPropertyNameChanged)]
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
