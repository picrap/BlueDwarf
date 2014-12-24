using System;
using BlueDwarf.Utility;

namespace BlueDwarf.Aspects
{
    public static class AutoNotifyPropertyChangedExtensions
    {
        public static object GetPropertyCategory(this ViewModel.ViewModel viewModel, string propertyName)
        {
            var notifyPropertyChangedAttribute = viewModel.GetType().GetProperty(propertyName).GetCustomAttribute<AutoNotifyPropertyChanged>();
            return notifyPropertyChangedAttribute.Category;
        }

        public static TCategory GetPropertyCategory<TCategory>(this ViewModel.ViewModel viewModel, string propertyName)
        {
            try
            {
                var category = GetPropertyCategory(viewModel, propertyName);
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