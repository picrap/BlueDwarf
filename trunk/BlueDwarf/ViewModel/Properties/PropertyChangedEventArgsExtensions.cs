// This is the blue dwarf
// more information at https://code.google.com/p/blue-dwarf/

namespace BlueDwarf.ViewModel.Properties
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Extensions to make it more handy (because we're pragmatic people)
    /// </summary>
    public static class PropertyChangedEventArgsExtensions
    {
        /// <summary>
        /// Gets the property category, as an object.
        /// </summary>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        /// <returns></returns>
        public static object GetCategory(this PropertyChangedEventArgs e)
        {
            var categoryPropertyChangedEventArgs = e as CategoryPropertyChangedEventArgs;
            if (categoryPropertyChangedEventArgs == null)
                return null;
            return categoryPropertyChangedEventArgs.Category;
        }

        /// <summary>
        /// Gets the property category, as the requested type (whose only pupose is to avoid casts in client calls).
        /// </summary>
        /// <typeparam name="TCategory">The type of the category.</typeparam>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        /// <returns></returns>
        public static TCategory GetCategory<TCategory>(this PropertyChangedEventArgs e)
        {
            try
            {
                var category = GetCategory(e);
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