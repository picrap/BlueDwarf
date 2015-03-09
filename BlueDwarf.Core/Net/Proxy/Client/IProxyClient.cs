// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Net.Proxy.Client
{
    using System;

    public interface IProxyClient
    {
        /// <summary>
        /// Validates and creates a route.
        /// </summary>
        /// <param name="proxyServers">The proxy servers.</param>
        /// <returns></returns>
        Route CreateRoute(params ProxyServer[] proxyServers);
    }
}