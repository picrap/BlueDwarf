
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BlueDwarf.Utility;

namespace BlueDwarf.Controls
{
    /// <summary>
    /// UrlBox.xaml code behind
    /// </summary>
    public partial class UrlBox
    {
        #region public dependency Uri Uri { get; set; }

        public static readonly DependencyProperty UriProperty = DependencyProperty.Register(
            "Uri", typeof(Uri), typeof(UrlBox), new PropertyMetadata(default(Uri), OnUriChanged));

        public Uri Uri
        {
            get { return (Uri)GetValue(UriProperty); }
            set { SetValue(UriProperty, value); }
        }

        #endregion

        #region public dependency string[] AllowedSchemes { get; set; }

        public static readonly DependencyProperty AllowedSchemesProperty = DependencyProperty.Register(
            "AllowedSchemes", typeof(string), typeof(UrlBox), new PropertyMetadata("http", OnAllowedSchemesChanged));

        public string AllowedSchemes
        {
            get { return (string)GetValue(AllowedSchemesProperty); }
            set { SetValue(AllowedSchemesProperty, value); }
        }

        #endregion

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

        private static void OnAllowedSchemesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var @this = (UrlBox)d;
            @this.UpdateAllowedSchemes();
        }

        private void UpdateAllowedSchemes()
        {
            UpdateLock(delegate
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
            });
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

        private static void OnUriChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var @this = (UrlBox)d;
            @this.WriteUri();
        }

        private bool _updating;

        /// <summary>
        /// Writes the URI to inner controls.
        /// </summary>
        private void WriteUri()
        {
            if (_updating)
                return;

            UpdateLock(delegate
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
            });
        }

        /// <summary>
        /// Reads the URI from inner controls and updates dependency property.
        /// </summary>
        private void ReadUri()
        {
            if (_updating)
                return;

            UpdateLock(delegate
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
                    Uri = new Uri(uriString);
                    return;
                }

                int port;
                if (!int.TryParse(PortTextBox.Text, out port) || port < 0)
                    return;
                var uriStringWithPort = string.Format("{0}://{1}:{2}", scheme, HostTextBox.Text, port);
                Uri = new Uri(uriStringWithPort);
            });
        }

        private void UpdateLock(Action action)
        {
            try
            {
                _updating = true;
                action();
            }
            finally
            {
                _updating = false;
            }
        }
    }
}
