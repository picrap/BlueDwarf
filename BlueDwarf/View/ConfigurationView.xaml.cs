// This is the blue dwarf
// more information at https://code.google.com/p/blue-dwarf/

namespace BlueDwarf.View
{
    using System.Diagnostics;
    using System.Windows.Navigation;

    /// <summary>
    /// Configuration view
    /// </summary>
    public partial class ConfigurationView
    {
        public ConfigurationView()
        {
            InitializeComponent();
        }

        private void OnHyperlinkRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
