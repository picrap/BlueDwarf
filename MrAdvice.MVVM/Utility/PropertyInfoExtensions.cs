namespace ArxOne.MrAdvice.Utility
{
    using System.Reflection;

    /// <summary>
    /// Extensions to property infos
    /// </summary>
    internal static class PropertyInfoExtensions
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