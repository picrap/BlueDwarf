// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Controls
{
    using View.Properties;

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
