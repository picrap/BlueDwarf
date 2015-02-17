// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.View.Properties
{
    using System;
    using System.Windows;
    using ArxOne.MrAdvice.Advice;
    using ArxOne.MrAdvice.Annotation;
    using Aspects;
    using Utility;
    using ViewModel.Properties;

    /// <summary>
    /// Marks a simple auto property to be bound to a DependencyProperty
    /// This attribute is handled by Postsharp, and by some unexplainable magic, works.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    [Priority(AspectPriority.DataHolder)]
    public class DependencyProperty : Attribute, IPropertyAdvice, IPropertyInfoAdvice
    {
        /// <summary>
        /// Gets or sets the default value for the dependency property.
        /// </summary>
        /// <value>
        /// The default value.
        /// </value>
        public object DefaultValue { get; set; }

        /// <summary>
        /// Gets or sets the notification type.
        /// </summary>
        /// <value>
        /// The notification.
        /// </value>
        public DependencyPropertyNotification Notification { get; set; }

        public void Advise(PropertyInfoAdviceContext context)
        {
            var propertyInfo = context.TargetProperty;
            propertyInfo.CreateDependencyProperty(DefaultValue, Notification);
        }

        public void Advise(PropertyAdviceContext context)
        {
            if (context.IsGetter)
            {
                var dependencyProperty = context.TargetProperty.GetDependencyProperty();
                var dependencyObject = (DependencyObject) context.Target;
                // yes, in the end, it is a GetValue()
                context.ReturnValue = dependencyObject.GetValue(dependencyProperty);
            }
            else
            {
                var dependencyProperty = context.TargetProperty.GetDependencyProperty();
                var dependencyObject = (DependencyObject)context.Target;
                var oldValue = dependencyObject.GetValue(dependencyProperty);
                var newValue = context.Value;
                // not sure it is necessary to check for a change
                if (!oldValue.SafeEquals(newValue))
                    dependencyObject.SetValue(dependencyProperty, newValue);
            }
        }
    }
}
