using System;

namespace BlueDwarf.Net.Proxy.Client
{
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