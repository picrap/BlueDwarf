// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Net.Proxy.Client
{
    /// <summary>
    /// Proxy client interface
    /// </summary>
    public interface IProxyClient
    {
        /// <summary>
        /// Creates a route.
        /// </summary>
        /// <param name="proxyServers">The proxy servers.</param>
        /// <returns></returns>
        Route CreateRoute(params ProxyServer[] proxyServers);
    }
}