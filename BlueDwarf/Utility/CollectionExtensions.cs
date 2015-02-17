// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Utility
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Windows.Controls;

    public static class CollectionExtensions
    {
        public static void AddRange<TItem>(this ICollection<TItem> collection, IEnumerable<TItem> items)
        {
            foreach (var item in items)
                collection.Add(item);
        }

        public static void AddRange(this ItemCollection collection, IEnumerable items)
        {
            foreach (var item in items)
                collection.Add(item);
        }
    }
}
