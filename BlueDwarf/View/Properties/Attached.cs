// This is the blue dwarf
// more information at https://code.google.com/p/blue-dwarf/
namespace BlueDwarf.View.Properties
{
    using System;
    using ArxOne.MrAdvice.Advice;
    using ArxOne.MrAdvice.Annotation;
    using Aspects;
    using ViewModel.Properties;

    /// <summary>
    /// Aspect for attached properties
    /// See Property for more information
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    [Priority(AspectPriority.DataHolder)]
    public class Attached : Attribute, IPropertyAdvice, IPropertyInfoAdvice
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

        /// <summary>
        /// The CurrentProperty is used by Property
        /// The syntax for using all of this is elegant, much more elegant than the implementation
        /// </summary>
        [ThreadStatic]
        internal static System.Windows.DependencyProperty CurrentProperty;

        private static readonly object[] NoParameter = new object[0];

        public void Advise(PropertyInfoAdviceContext context)
        {
            var propertyInfo = context.TargetProperty;
            propertyInfo.CreateDependencyProperty(DefaultValue, Notification);
            if (propertyInfo.GetValue(null, NoParameter) == null)
                propertyInfo.SetValue(null, Activator.CreateInstance(propertyInfo.PropertyType), NoParameter);
        }

        public void Advise(PropertyAdviceContext context)
        {
            CurrentProperty = context.TargetProperty.GetDependencyProperty();
            context.Proceed();
        }
    }
}