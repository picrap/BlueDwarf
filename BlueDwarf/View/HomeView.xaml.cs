// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf

namespace BlueDwarf.View
{
    using System;
    using System.Windows;
    using ArxOne.MrAdvice.MVVM.Properties;
    using Configuration;
    using Microsoft.Practices.Unity;
    using DependencyProperty = ArxOne.MrAdvice.MVVM.Properties.DependencyProperty;

    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView
    {
        [Dependency]
        // ReSharper disable once UnusedMember.Global
        public IPersistence Persistence { get; set; }

        [DependencyProperty(Notification = DependencyPropertyNotification.OnPropertyNameChanged)]
        public Uri View { get; set; }

        [Persistent("View", AutoSave = true)]
        public Uri CurrentView { get; set; }

        public HomeView()
        {
            InitializeComponent();
            Loaded += OnLoad;
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            View = CurrentView;
        }

        /// <summary>
        /// Called when [view changed].
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public void OnViewChanged()
        {
            CurrentView = View;
        }
    }
}
