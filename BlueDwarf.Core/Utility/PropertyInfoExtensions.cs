// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
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
