// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf

namespace BlueDwarf.Utility
{
    using System;
    using System.Reflection;
    using ArxOne.MrAdvice.Advice;
    using ArxOne.MrAdvice.Introduction;

    [AttributeUsage(AttributeTargets.Method)]
    public class ExclusiveUpdate : Attribute, IMethodAdvice, IMethodInfoAdvice
    {
        public IntroducedField<bool> Locked { get; set; }

        public void Advise(MethodInfoAdviceContext context)
        {
            var methodInfo = context.TargetMethod as MethodInfo;
            if (methodInfo != null && methodInfo.ReturnType != (typeof(void)))
                throw new InvalidOperationException("ExclusiveUpdate can only be applied to void methods");
        }

        public void Advise(MethodAdviceContext context)
        {
            var locked = Locked[context];
            if (locked)
                return;
            try
            {
                Locked[context] = true;
                context.Proceed();
            }
            finally
            {
                Locked[context] = false;
            }
        }
    }
}
