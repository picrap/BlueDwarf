// This is the blue dwarf
// more information at https://code.google.com/p/blue-dwarf/

namespace BlueDwarf.Net.Name
{
    using System.Net;
    using Proxy.Client;

    /// <summary>
    /// Name resolution interface
    /// (yes, in other words, a simple DNS client)
    /// </summary>
    public interface INameResolver
    {
        /// <summary>
        /// Resolves the specified name using the given route.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="route">The route.</param>
        /// <returns></returns>
        IPAddress Resolve(string name, ProxyRoute route);
    }
}