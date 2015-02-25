// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Configuration
{
    using System;
    using ArxOne.MrAdvice.Advice;
    using ArxOne.MrAdvice.Annotation;
    using Aspects;

    [AttributeUsage(AttributeTargets.Property)]
    [Priority(AspectPriority.DataHolder)]
    public class Persistent : Attribute, IPropertyAdvice
    {
        /// <summary>
        /// Gets or sets the name under which the property is serialized.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this property is automatically saved as soon as it changes.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [automatic save]; otherwise, <c>false</c>.
        /// </value>
        public bool AutoSave { get; set; }

        /// <summary>
        /// Gets or sets the default value.
        /// </summary>
        /// <value>
        /// The default value.
        /// </value>
        public object DefaultValue { get; set; }

        [NonSerialized]
        private PropertyImporter<IPersistence> _persistenceProperty;

        public Persistent(string name)
        {
            Name = name;
        }

        public void Advise(PropertyAdviceContext context)
        {
            if (_persistenceProperty == null)
                _persistenceProperty = new PropertyImporter<IPersistence>(context.TargetProperty.DeclaringType);
            var persistence = _persistenceProperty.Get(context.Target);
            if (context.IsGetter)
                context.ReturnValue = persistence.GetValue(Name, context.TargetProperty.PropertyType, DefaultValue);
            else
                persistence.SetValue(Name, context.Value, AutoSave);
        }
    }
}
