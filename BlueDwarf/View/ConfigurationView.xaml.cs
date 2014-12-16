using System.ComponentModel;
using System.Windows;

namespace BlueDwarf.View
{
    /// <summary>
    /// Configuration view
    /// </summary>
    public partial class ConfigurationView
    {
        #region public dependency bool HideOnClose { get; set; }

        public static readonly DependencyProperty HideOnCloseProperty = DependencyProperty.Register(
            "HideOnClose", typeof(bool), typeof(ConfigurationView), new PropertyMetadata(true));

        public bool HideOnClose
        {
            get { return (bool)GetValue(HideOnCloseProperty); }
            set { SetValue(HideOnCloseProperty, value); }
        }

        #endregion

        public ConfigurationView()
        {
            InitializeComponent();
        }
    }
}