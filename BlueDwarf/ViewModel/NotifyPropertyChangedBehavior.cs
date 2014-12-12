
using System;
using System.Collections.Generic;
using System.Linq;
using BlueDwarf.Annotations;
using BlueDwarf.Utility;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace BlueDwarf.ViewModel
{
    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    public class NotifyPropertyChangedBehavior : IInterceptionBehavior
    {
        public bool WillExecute
        {
            get { return true; }
        }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            if (input.MethodBase.IsPropertySetter())
            {
                var propertyInfo = input.MethodBase.GetProperty();
                if (propertyInfo != null)
                {
                    var attribute = propertyInfo.GetCustomAttributes(typeof(NotifyPropertyChangedAttribute), true).Cast<NotifyPropertyChangedAttribute>().SingleOrDefault();
                    if (attribute != null)
                    {
                        var oldValue = propertyInfo.GetValue(input.Target, new object[0]);
                        var newValue = input.Arguments[0];
                        if (!oldValue.SafeEquals(newValue))
                        {
                            var viewModel = input.Target as ViewModel;
                            if (viewModel != null)
                            {
                                var result = getNext()(input, getNext);
                                viewModel.OnPropertyChanged(propertyInfo.Name);
                                return result;
                            }
                        }
                    }
                }
            }
            return getNext()(input, getNext);
        }

        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return Type.EmptyTypes;
        }
    }
}
