using System;
using BlueDwarf.Utility;

namespace BlueDwarf.ViewModel
{
    public static class NotifyPropertyChangedAttributeExtensions
    {
        public static object GetCategory(this ViewModel viewModel, string propertyName)
        {
            var notifyPropertyChangedAttribute = viewModel.GetType().GetProperty(propertyName).GetCustomAttribute<NotifyPropertyChangedAttribute>();
            return notifyPropertyChangedAttribute.Category;
        }

        public static TCategory GetCategory<TCategory>(this ViewModel viewModel, string propertyName)
        {
            try
            {
                var category = GetCategory(viewModel, propertyName);
                if (category != null)
                    return (TCategory)category;
            }
            catch (InvalidCastException)
            {
            }
            return default(TCategory);
        }
    }
}