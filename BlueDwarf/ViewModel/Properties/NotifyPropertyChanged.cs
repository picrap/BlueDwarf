// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.ViewModel.Properties
{
    using System;
    using System.Linq;
    using ArxOne.MrAdvice.Advice;
    using ArxOne.MrAdvice.Annotation;
    using Aspects;
    using Utility;

    /// <summary>
    /// Invokes notify property changed... If the property has actually changed
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    [Priority(AspectPriority.Notification)]
    public class NotifyPropertyChanged : Attribute, IPropertyAdvice
    {
        /// <summary>
        /// Gets or sets the category, a custom value used by notifications
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        public object Category { get; set; }

        public void Advise(PropertyAdviceContext context)
        {
            if (context.IsGetter)
                context.Proceed();
            else
            {
                var oldValue = context.TargetProperty.GetValue(context.Target, context.Index.ToArray());

                // first, set the value
                context.Proceed();

                // then, notify, if it has changed
                var newValue = context.Value;
                if (!oldValue.SafeEquals(newValue))
                {
                    var viewModel = (ViewModel)context.Target;
                    viewModel.OnPropertyChanged(context.TargetProperty.Name, Category);
                }
            }
        }
    }
}
