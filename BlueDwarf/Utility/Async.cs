// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Utility
{
    using System;
    using System.Reflection;
    using System.Threading;
    using ArxOne.MrAdvice.Advice;

    /// <summary>
    /// Allows to invoke a method asynchronously (here, in a background thread)
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class Async : Attribute, IMethodAdvice, IMethodInfoAdvice
    {
        public bool KillExisting { get; set; }

        [NonSerialized]
        private Thread _thread;

        public void Advise(MethodInfoAdviceContext context)
        {
            var methodInfo = context.TargetMethod as MethodInfo;
            if (methodInfo == null)
                return;
            if (methodInfo.ReturnType != typeof(void))
                throw new InvalidOperationException("Impossible to run asynchronously a non-void method (you MoFo!)");
        }

        public void Advise(MethodAdviceContext context)
        {
            if (KillExisting && _thread != null && _thread.IsAlive)
                _thread.Abort();
            _thread = new Thread(context.Proceed) { IsBackground = true, Name = context.TargetMethod.Name };
            _thread.Start();
        }
    }
}
