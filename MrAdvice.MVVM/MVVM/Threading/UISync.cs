
namespace ArxOne.MrAdvice.MVVM.Threading
{
    using System;
    using System.Reflection;
    using System.Windows;
    using ArxOne.MrAdvice.Advice;

    /// <summary>
    /// Allows to invoke a method asynchronously (here, in a background thread)
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class UISync : Attribute, IMethodAdvice, IMethodInfoAdvice
    {
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
            Invoke(context.Proceed);
        }

        public static void Invoke(Action action)
        {
            var dispatcher = Application.Current.Dispatcher;
            if (dispatcher.CheckAccess())
                action();
            else
                dispatcher.BeginInvoke(action);
        }
    }
}
