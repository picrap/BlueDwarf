
namespace ArxOne.MrAdvice.Utility
{
    using System;
    using Microsoft.Practices.ServiceLocation;

    internal static class ServiceLocatorExtensions
    {
        public static object GetOrCreateInstance(this IServiceLocator serviceLocator, Type instanceType)
        {
            try
            {
                return serviceLocator.GetInstance(instanceType);
            }
            catch (ActivationException)
            {
                return Activator.CreateInstance(instanceType);
            }
        }
    }
}
