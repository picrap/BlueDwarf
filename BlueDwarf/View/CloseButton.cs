
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using BlueDwarf.Utility;

namespace BlueDwarf.View
{
    public class CloseButton : FrameworkElement
    {
        public ICommand Command { get; set; }

        public object CommandParameter { get; set; }

        public CloseButton()
        {
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var parent = this.GetSelfAndParents().OfType<Window>().FirstOrDefault();
            if (parent != null)
                parent.Closing += OnParentClosing;
        }

        private void OnParentClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;

            if (Command != null && Command.CanExecute(CommandParameter))
                Command.Execute(CommandParameter);
        }
    }
}
