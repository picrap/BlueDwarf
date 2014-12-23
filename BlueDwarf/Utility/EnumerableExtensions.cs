using System.Collections.Generic;

namespace BlueDwarf.Utility
{
    public static class EnumerableExtensions
    {
        public static int IndexOf<TItem>(this IEnumerable<TItem> collection, TItem find, Comparer<TItem> comparer = null)
        {
            comparer = comparer ?? Comparer<TItem>.Default;
            int index = 0;
            foreach (var item in collection)
            {
                if (comparer.Compare(item, find) == 0)
                    return index;
                index++;
            }
            return -1;
        }
    }
}