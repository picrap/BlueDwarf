// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf

namespace BlueDwarf.View
{
    using System.Linq;
    using System.Windows;

    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HomeView"/> class.
        /// </summary>
        public HomeView()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        /// <summary>
        /// Called when page is loaded.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (ContentSource == null)
                ContentSource = MenuLinkGroups.First().Links.First().Source;
        }
    }
}
