using System;
using BlueDwarf.Utility;
using PostSharp.Aspects;
using PostSharp.Extensibility;

namespace BlueDwarf.Aspects
{
    /// <summary>
    /// Invokes notify property changed... If the property has actually changed
    /// </summary>
    [Serializable]
    [MulticastAttributeUsage(MulticastTargets.Property, AllowMultiple = false, TargetMemberAttributes = MulticastAttributes.NonAbstract | MulticastAttributes.NonLiteral, PersistMetaData = true)]
    public class AutoNotifyPropertyChanged : LocationInterceptionAspect
    {
        public object Category { get; set; }

        /// <summary>
        /// Method invoked <i>instead</i> of the <c>Set</c> semantic of the field or property to which the current aspect is applied,
        /// i.e. when the value of this field or property is changed.
        /// </summary>
        /// <param name="args">Advice arguments.</param>
        public override void OnSetValue(LocationInterceptionArgs args)
        {
            var oldValue = args.Location.PropertyInfo.GetValue(args.Instance, args.Index.ToArray());

            // first, set the value
            args.ProceedSetValue();

            // then, notify, if it has changed
            var newValue = args.Value;
            if (!oldValue.SafeEquals(newValue))
            {
                var viewModel = (ViewModel.ViewModel)args.Instance;
                viewModel.OnPropertyChanged(args.Location.PropertyInfo.Name);
            }
        }
    }
}
