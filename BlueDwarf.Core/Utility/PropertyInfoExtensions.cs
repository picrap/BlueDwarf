using System.Reflection;

namespace BlueDwarf.Utility
{
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
