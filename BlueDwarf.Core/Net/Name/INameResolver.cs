using System.Net;
using BlueDwarf.Net.Proxy.Client;

namespace BlueDwarf.Net.Name
{
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