using BlueDwarf.Aspects;

namespace BlueDwarf.Controls
{
    /// <summary>
    /// Logique d'interaction pour Loading.xaml
    /// </summary>
    public partial class Loading 
    {
        [DependencyProperty]
        public bool Show { get; set; }

        public Loading()
        {
            InitializeComponent();
        }
    }
}
