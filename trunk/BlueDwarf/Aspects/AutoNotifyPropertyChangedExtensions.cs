using System;
using BlueDwarf.Utility;

namespace BlueDwarf.Aspects
{
    /// <summary>
    /// Extensions to make it more handy (because we're pragmatic people)
    /// </summary>
    public static class AutoNotifyPropertyChangedExtensions
    {
        /// <summary>
        /// Gets the property category, as an object.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public static object GetPropertyCategory(this ViewModel.ViewModel viewModel, string propertyName)
        {
            var notifyPropertyChangedAttribute = viewModel.GetType().GetProperty(propertyName).GetCustomAttribute<AutoNotifyPropertyChanged>();
            return notifyPropertyChangedAttribute.Category;
        }

        /// <summary>
        /// Gets the property category, as the requested type (whose only pupose is to avoid casts in client calls).
        /// </summary>
        /// <typeparam name="TCategory">The type of the category.</typeparam>
        /// <param name="viewModel">The view model.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
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