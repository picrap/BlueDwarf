﻿using System;
using BlueDwarf.Utility;
using PostSharp.Aspects;
using PostSharp.Extensibility;

namespace BlueDwarf.Aspects
{
    using PostSharp.Aspects.Configuration;
    using PostSharp.Reflection;

    /// <summary>
    /// Invokes notify property changed... If the property has actually changed
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    [MulticastAttributeUsage(MulticastTargets.Property, PersistMetaData = true)]
    [Serializable]
    [LocationInterceptionAspectConfigurationAttribute(AspectPriority = 10)]
    public class NotifyPropertyChanged : Aspect, ILocationInterceptionAspect
    {
        /// <summary>
        /// Gets or sets the category, a custom value used by notifications
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        public object Category { get; set; }

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
            args.ProceedGetValue();
        }

        /// <summary>
        /// Method invoked <i>instead</i> of the <c>Set</c> semantic of the field or property to which the current aspect is applied,
        /// i.e. when the value of this field or property is changed.
        /// </summary>
        /// <param name="args">Advice arguments.</param>
        public void OnSetValue(LocationInterceptionArgs args)
        {
            var oldValue = args.Location.PropertyInfo.GetValue(args.Instance, args.Index.ToArray());

            // first, set the value
            args.ProceedSetValue();

            // then, notify, if it has changed
            var newValue = args.Value;
            if (!oldValue.SafeEquals(newValue))
            {
                var viewModel = (ViewModel.ViewModel)args.Instance;
                viewModel.OnPropertyChanged(args.Location.PropertyInfo.Name, Category);
            }
        }
    }
}