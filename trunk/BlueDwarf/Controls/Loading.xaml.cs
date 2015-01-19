// This is the blue dwarf
// more information at https://code.google.com/p/blue-dwarf/

namespace BlueDwarf.Controls
{
    using View;
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
