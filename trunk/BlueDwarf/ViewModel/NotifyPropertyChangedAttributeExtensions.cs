using BlueDwarf.Utility;

namespace BlueDwarf.ViewModel
{
    public static class NotifyPropertyChangedAttributeExtensions
    {
        public static object GetCategory(this ViewModel viewModel, string propertyName)
        {
            var notifyPropertyChangedAttribute = viewModel.GetType().GetCustomAttribute<NotifyPropertyChangedAttribute>();
            return notifyPropertyChangedAttribute.Category;
        }

        public static TCategory GetCategory<TCategory>(this ViewModel viewModel, string propertyName)
        {
            return (TCategory)GetCategory(viewModel, propertyName);
        }
    }
}