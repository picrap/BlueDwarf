// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.ViewModel.Properties
{
    using System.ComponentModel;

    public class CategoryPropertyChangedEventArgs : PropertyChangedEventArgs
    {
        public object Category { get; private set; }

        public CategoryPropertyChangedEventArgs(string propertyName, object category)
            : base(propertyName)
        {
            Category = category;
        }
    }
}
