// This is the blue dwarf
// more information at https://code.google.com/p/blue-dwarf/

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
