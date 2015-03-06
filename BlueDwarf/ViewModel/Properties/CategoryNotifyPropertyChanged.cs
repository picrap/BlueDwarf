// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf

namespace BlueDwarf.ViewModel.Properties
{
    using ArxOne.MrAdvice.Advice;
    using ArxOne.MrAdvice.Annotation;
    using ArxOne.MrAdvice.MVVM.Properties;

    [Priority(AspectPriority.Notification)]
    public  class CategoryNotifyPropertyChanged: NotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        public object Category { get; set; }
    }
}
