// This is the blue dwarf
// more information at https://code.google.com/p/blue-dwarf/

namespace BlueDwarf.Utility
{
    using System.Reflection;

    public static class PropertyInfoExtensions
    {
        public static bool IsStatic(this PropertyInfo propertyInfo)
        {
            var getMethod = propertyInfo.GetGetMethod();
            if (getMethod != null)
                return getMethod.IsStatic;
            return propertyInfo.GetSetMethod().IsStatic;
        }
    }
}
