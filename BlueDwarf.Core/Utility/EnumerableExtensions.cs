// This is the blue dwarf
// more information at https://code.google.com/p/blue-dwarf/

namespace BlueDwarf.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class EnumerableExtensions
    {
        public static IEnumerable<TItem> WhereNonNull<TItem>(this IEnumerable<TItem> items)
            where TItem : class
        {
            return items.Where(i => i != null);
        }

        public static IEnumerable<TResult> SelectNonNull<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
            where TResult : class
        {
            return source.Select(selector).WhereNonNull();
        }
    }
}
