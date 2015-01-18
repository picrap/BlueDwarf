
namespace BlueDwarf.Configuration
{
    using System;
    using PostSharp.Aspects;
    using PostSharp.Aspects.Configuration;
    using PostSharp.Extensibility;
    using PostSharp.Reflection;

    [AttributeUsage(AttributeTargets.Property)]
    [MulticastAttributeUsage(MulticastTargets.Property, PersistMetaData = true)]
    [Serializable]
    [LocationInterceptionAspectConfigurationAttribute(AspectPriority = 100)]
    public class Persistent : Aspect, ILocationInterceptionAspect
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
        public object Default { get; set; }

        public Persistent(string name)
        {
            Name = name;
        }

        [Obsolete("Serialization-only ctor")]
        public Persistent()
        {
        }

        public void RuntimeInitialize(LocationInfo locationInfo)
        {
        }

        /// <summary>
        /// Method invoked <i>instead</i> of the <c>Get</c> semantic of the field or property to which the current aspect is applied,
        /// i.e. when the value of this field or property is retrieved.
        /// </summary>
        /// <param name="args">Advice arguments.</param>
        public void OnGetValue(LocationInterceptionArgs args)
        {
            args.Value = Persistence.Current.GetValue(Name, args.Location.PropertyInfo.PropertyType, Default);
        }

        /// <summary>
        /// Method invoked <i>instead</i> of the <c>Set</c> semantic of the field or property to which the current aspect is applied,
        /// i.e. when the value of this field or property is changed.
        /// </summary>
        /// <param name="args">Advice arguments.</param>
        public void OnSetValue(LocationInterceptionArgs args)
        {
            Persistence.Current.SetValue(Name, args.Value, AutoSave);
        }
    }
}
