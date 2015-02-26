// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf

namespace BlueDwarf.Utility
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Extensions to member info
    /// </summary>
    public static class MemberInfoExtensions
    {
        /// <summary>
        /// Sets the member value.
        /// </summary>
        /// <param name="memberInfo">The member information.</param>
        /// <param name="target">The target.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="System.InvalidOperationException"></exception>
        public static void SetMemberValue(this MemberInfo memberInfo, object target, object value)
        {
            var propertyInfo = memberInfo as PropertyInfo;
            if (propertyInfo != null)
            {
                propertyInfo.SetValue(target, value, new object[0]);
                return;
            }

            var fieldInfo = memberInfo as FieldInfo;
            if (fieldInfo != null)
            {
                fieldInfo.SetValue(target, value);
                return;
            }

            throw new InvalidOperationException();
        }
    }
}
