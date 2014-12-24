using BlueDwarf.Aspects;

namespace BlueDwarf.Controls
{
    /// <summary>
    /// Logique d'interaction pour Loading.xaml
    /// </summary>
    public partial class Loading 
    {
        [AutoDependencyProperty]
        public bool Show { get; set; }

        public Loading()
        {
            InitializeComponent();
        }
    }
}
