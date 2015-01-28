
namespace BlueDwarf.View
{
    using System.Diagnostics;
    using System.Windows.Navigation;

    /// <summary>
    /// Logique d'interaction pour ProxyPickerView.xaml
    /// </summary>
    public partial class ProxyPickerView 
    {
        public ProxyPickerView()
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
