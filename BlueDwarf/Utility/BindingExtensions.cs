// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf

namespace BlueDwarf.Utility
{
    using System.Windows.Data;

    internal static class BindingExtensions
    {
        public static object GetValue(this Binding binding, object dataItem)
        {
            if (binding == null || dataItem == null)
                return null;

            var bindingPath = binding.Path.Path;
            var properties = bindingPath.Split('.');

            var currentObject = dataItem;

            foreach (string property in properties)
            {
                var currentType = currentObject.GetType();
                var propertyInfo = currentType.GetProperty(property);
                if (propertyInfo == null)
                {
                    currentObject = null;
                    break;
                }
                currentObject = propertyInfo.GetValue(currentObject, null);
                if (currentObject == null)
                {
                    break;
                }
            }

            return currentObject;
        }
    }
}
