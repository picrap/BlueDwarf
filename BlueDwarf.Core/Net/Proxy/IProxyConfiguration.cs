// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Net.Proxy
{
    using System;

    public interface IProxyConfiguration
    {
        /// <summary>
        /// Sets the application-wide proxy.
        /// </summary>
        /// <param name="proxy">The proxy.</param>
        void SetApplicationProxy(Uri proxy);
    }
}