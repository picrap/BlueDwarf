// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Controls
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using ArxOne.MrAdvice.MVVM.Properties;
    using Utility;
    using DependencyProperty = ArxOne.MrAdvice.MVVM.Properties.DependencyProperty;

    /// <summary>
    /// UrlBox.xaml code behind
    /// </summary>
    public partial class UrlBox
    {
        [DependencyProperty(Notification = DependencyPropertyNotification.OnPropertyNameChanged)]
        public Uri Uri { get; set; }

        [DependencyProperty(DefaultValue = "http", Notification = DependencyPropertyNotification.OnPropertyNameChanged)]
        public string AllowedSchemes { get; set; }

        private string[] AllowedSchemesArray
        {
            get { return AllowedSchemes.Split(';'); }
        }

        public UrlBox()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            UpdateAllowedSchemes();
        }

        public void OnAllowedSchemesChanged()
        {
            UpdateAllowedSchemes();
        }

        [ExclusiveUpdate]
        private void UpdateAllowedSchemes()
        {
            var allowedSchemes = AllowedSchemesArray;

            var selectedItem = SchemeComboxBox.SelectedItem;
            SchemeComboxBox.Items.Clear();
            SchemeComboxBox.Items.AddRange(allowedSchemes);
            SchemeComboxBox.SelectedItem = selectedItem;
            SchemeTextBlock.Text = allowedSchemes.FirstOrDefault();
            var manySchemes = allowedSchemes.Length > 1;
            SchemeTextBlock.Visibility = manySchemes ? Visibility.Collapsed : Visibility.Visible;
            SchemeComboxBox.Visibility = manySchemes ? Visibility.Visible : Visibility.Collapsed;
        }

        private void OnSchemeChanged(object sender, SelectionChangedEventArgs e)
        {
            ReadUri();
        }

        private void OnHostChanged(object sender, TextChangedEventArgs e)
        {
            ReadUri();
        }

        private void OnPortChanged(object sender, TextChangedEventArgs e)
        {
            ReadUri();
        }

        public void OnUriChanged()
        {
            WriteUri();
        }

        /// <summary>
        /// Writes the URI to inner controls.
        /// </summary>
        [ExclusiveUpdate]
        private void WriteUri()
        {
            var uri = Uri;
            if (uri != null)
            {
                SchemeTextBlock.Text = uri.Scheme;
                SchemeComboxBox.SelectedItem = SchemeComboxBox.Items.OfType<string>().FirstOrDefault(s => s == uri.Scheme);
                HostTextBox.Text = uri.Host;
                if (!uri.IsDefaultPort)
                    PortTextBox.Text = uri.Port.ToString();
            }
            else
            {
                var allowedSchemes = AllowedSchemesArray;
                SchemeTextBlock.Text = allowedSchemes.FirstOrDefault();
                SchemeComboxBox.SelectedIndex = 0;
                HostTextBox.Text = null;
                PortTextBox.Text = null;
            }
        }

        /// <summary>
        /// Reads the URI from inner controls and updates dependency property.
        /// </summary>
        [ExclusiveUpdate]
        private void ReadUri()
        {
            if (HostTextBox.Text.IsNullOrEmpty())
            {
                Uri = null;
                return;
            }

            var selectedItem = SchemeComboxBox.SelectedItem as string;
            var scheme = selectedItem ?? SchemeTextBlock.Text;
            if (PortTextBox.Text.IsNullOrEmpty())
            {
                var uriString = string.Format("{0}://{1}", scheme, HostTextBox.Text);
                try
                {
                    Uri = new Uri(uriString);
                }
                catch (UriFormatException)
                { }
                return;
            }

            int port;
            if (!int.TryParse(PortTextBox.Text, out port) || port < 0)
                return;
            var uriStringWithPort = string.Format("{0}://{1}:{2}", scheme, HostTextBox.Text, port);
            try
            {
                Uri = new Uri(uriStringWithPort);
            }
            catch (UriFormatException) { }
        }
    }
}
