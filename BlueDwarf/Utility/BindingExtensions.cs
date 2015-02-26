// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf

namespace BlueDwarf.Utility
{
    using System.Windows.Data;

    /// <summary>
    /// Extensions to binding
    /// </summary>
    internal static class BindingExtensions
    {
        /// <summary>
        /// Gets the value of binding related to data context.
        /// </summary>
        /// <param name="binding">The binding.</param>
        /// <param name="dataContext">The data item.</param>
        /// <returns></returns>
        public static object GetValue(this Binding binding, object dataContext)
        {
            if (binding == null || dataContext == null)
                return null;

            var bindingPath = binding.Path.Path;
            var properties = bindingPath.Split('.');

            var currentObject = dataContext;

            foreach (var property in properties)
            {
                var currentType = currentObject.GetType();
                var propertyInfo = currentType.GetProperty(property);
                if (propertyInfo == null)
                    return null;
                currentObject = propertyInfo.GetValue(currentObject, null);
                if (currentObject == null)
                    return null;
            }

            return currentObject;
        }
    }
}
