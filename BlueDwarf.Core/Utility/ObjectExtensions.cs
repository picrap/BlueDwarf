// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Utility
{
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