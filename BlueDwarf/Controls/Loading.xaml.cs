// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Controls
{
    using DependencyProperty = ArxOne.MrAdvice.MVVM.Properties.DependencyProperty;

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
