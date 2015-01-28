
namespace BlueDwarf.Utility
{
    using System;
    using System.Reflection;
    using PostSharp.Aspects;
    using PostSharp.Aspects.Configuration;
    using PostSharp.Extensibility;

    [AttributeUsage(AttributeTargets.Method)]
    [MulticastAttributeUsage(MulticastTargets.Method, PersistMetaData = true)]
    [Serializable]
    [MethodInterceptionAspectConfiguration]
    public class ExclusiveUpdate : Aspect, IMethodInterceptionAspect
    {
        public void RuntimeInitialize(MethodBase method)
        {
            var methodInfo = method as MethodInfo;
            if (methodInfo != null && methodInfo.ReturnType != (typeof(void)))
                throw new InvalidOperationException("ExclusiveUpdate can only be applied to void methods");
        }

        public void OnInvoke(MethodInterceptionArgs args)
        {
            var locked = InstanceData.GetData<bool>(args.Instance, "locked");
            if (locked)
                return;
            try
            {
                InstanceData.SetData(args.Instance, "locked", true);
                args.Proceed();
            }
            finally
            {
                InstanceData.SetData(args.Instance, "locked", false);
            }
        }
    }
}
