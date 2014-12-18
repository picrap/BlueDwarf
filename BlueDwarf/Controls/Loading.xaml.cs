using System.Windows;
using System.Windows.Controls;

namespace BlueDwarf.Controls
{
    /// <summary>
    /// Logique d'interaction pour Loading.xaml
    /// </summary>
    public partial class Loading 
    {
        public static readonly DependencyProperty ShowProperty = DependencyProperty.Register(
            "Show", typeof (bool), typeof (Loading), new PropertyMetadata(default(bool)));

        public bool Show
        {
            get { return (bool) GetValue(ShowProperty); }
            set { SetValue(ShowProperty, value); }
        }

        public Loading()
        {
            InitializeComponent();
        }
    }
}
