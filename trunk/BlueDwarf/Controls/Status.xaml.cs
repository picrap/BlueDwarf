﻿// This is the blue dwarf
// more information at https://code.google.com/p/blue-dwarf/

namespace BlueDwarf.Controls
{
    using System.Windows;
    using ViewModel.Properties;

    public partial class Status
    {
        [View.Properties.DependencyProperty(Notification = DependencyPropertyNotification.OnPropertyNameChanged)]
        public StatusCode Code { get; set; }

        public Status()
        {
            InitializeComponent();
        }

        public void OnCodeChanged()
        {
            OK.Visibility = Code == StatusCode.OK ? Visibility.Visible : Visibility.Collapsed;
            Error.Visibility = Code == StatusCode.Error ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
