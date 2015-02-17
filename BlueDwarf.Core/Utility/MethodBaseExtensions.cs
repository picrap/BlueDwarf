// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Utility
{
    using System.Reflection;

    /// <summary>
    /// Extensions to MethodBase
    /// </summary>
    public static class MethodBaseExtensions
    {
        /// <summary>
        /// Determines whether the method is a property setter.
        /// </summary>
        /// <param name="methodBase">The method base.</param>
        /// <returns></returns>
        public static bool IsPropertySetter(this MethodBase methodBase)
        {
            return methodBase.IsSpecialName && methodBase.Name.StartsWith("set_");
        }

        /// <summary>
        /// Gets the property whose provided method is a getter/setter.
        /// </summary>
        /// <param name="methodBase">The method base.</param>
        /// <returns></returns>
        public static PropertyInfo GetProperty(this MethodBase methodBase)
        {
            if (methodBase.IsSpecialName && (methodBase.Name.StartsWith("get_") || methodBase.Name.StartsWith("set_")))
                return methodBase.ReflectedType.GetProperty(methodBase.Name.Substring(4));
            return null;
        }
    }
}