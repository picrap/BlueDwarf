
namespace BlueDwarf.Utility
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Extensions to object
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Equals, with null support.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns></returns>
        public static bool SafeEquals(this object a, object b)
        {
            if (a == null || b == null)
                return (a == null) == (b == null);
            return a.Equals(b);
        }
    }
}