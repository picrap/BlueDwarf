// This is the blue dwarf
// more information at https://code.google.com/p/blue-dwarf/

namespace BlueDwarf.Utility
{
    using System;
    using System.Reflection;
    using System.Threading;
    using System.Windows;
    using PostSharp.Aspects;
    using PostSharp.Aspects.Configuration;
    using PostSharp.Extensibility;

    /// <summary>
    /// Allows to invoke a method asynchronously (here, in a background thread)
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    [MulticastAttributeUsage(MulticastTargets.Method, PersistMetaData = true)]
    [Serializable]
    [MethodInterceptionAspectConfiguration]
    public class UISync : Aspect, IMethodInterceptionAspect
    {
        public void RuntimeInitialize(MethodBase method)
        {
            var methodInfo = method as MethodInfo;
            if (methodInfo == null)
                return;
            if (methodInfo.ReturnType != typeof(void))
                throw new InvalidOperationException("Impossible to run asynchronously a non-void method (you MoFo!)");
        }

        public void OnInvoke(MethodInterceptionArgs args)
        {
            var dispatcher = Application.Current.Dispatcher;
            if (dispatcher.CheckAccess())
                args.Proceed();
            else
            {
                Delegate proceed = new Action(args.Proceed);
                dispatcher.BeginInvoke(proceed);
            }
        }
    }
}
