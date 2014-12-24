﻿
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BlueDwarf.Aspects;
using BlueDwarf.Utility;

namespace BlueDwarf.Controls
{
    /// <summary>
    /// UrlBox.xaml code behind
    /// </summary>
    public partial class UrlBox
    {
        [AutoDependencyProperty(Notification = AutoDependencyPropertyNotification.OnPropertyNameChanged)]
        public Uri Uri { get; set; }

        [AutoDependencyProperty(DefaultValue = "http", Notification = AutoDependencyPropertyNotification.OnPropertyNameChanged)]
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

        public void OnUriChanged()
        {
            WriteUri();
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
