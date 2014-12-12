using System.Reflection;

namespace BlueDwarf.Utility
{
    public static class MethodBaseExtensions
    {
        public static bool IsPropertySetter(this MethodBase methodBase)
        {
            return methodBase.IsSpecialName && methodBase.Name.StartsWith("set_");
        }

        public static PropertyInfo GetProperty(this MethodBase methodBase)
        {
            if (methodBase.IsSpecialName && (methodBase.Name.StartsWith("get_") || methodBase.Name.StartsWith("set_")))
                return methodBase.ReflectedType.GetProperty(methodBase.Name.Substring(4));
            return null;
        }
    }
}