using System;
using BlueDwarf.Utility;
using PostSharp.Aspects;
using PostSharp.Extensibility;

namespace BlueDwarf.Aspects
{
    [Serializable]
    [MulticastAttributeUsage(MulticastTargets.Property, AllowMultiple = false, TargetMemberAttributes = MulticastAttributes.NonAbstract | MulticastAttributes.NonLiteral, PersistMetaData = true)]
    public class AutoNotifyPropertyChanged : LocationInterceptionAspect
    {
        public object Category { get; set; }

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
