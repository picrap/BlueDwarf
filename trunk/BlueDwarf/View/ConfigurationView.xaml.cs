using System.Diagnostics;
using System.Windows.Navigation;

namespace BlueDwarf.View
{
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
