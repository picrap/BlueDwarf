// This is the blue dwarf
// more information at https://code.google.com/p/blue-dwarf/

namespace BlueDwarf.Net.Proxy.Client
{
    using System;

    public interface IProxyClient
    {
        /// <summary>
        /// Validates and creates a route.
        /// </summary>
        /// <param name="targetHost"></param>
        /// <param name="targetPort"></param>
        /// <param name="proxyServers">The proxy servers.</param>
        /// <returns></returns>
        ProxyRoute CreateRoute(string targetHost, int targetPort, params Uri[] proxyServers);
    }
}