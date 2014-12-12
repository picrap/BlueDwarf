using System;

namespace BlueDwarf.Net.Proxy.Client
{
    public interface IProxyClient
    {
        /// <summary>
        /// Validates and creates a route.
        /// </summary>
        /// <param name="testTarget">The test target.</param>
        /// <param name="proxyServers">The proxy servers.</param>
        /// <returns></returns>
        ProxyRoute CreateRoute(Uri testTarget, params Uri[] proxyServers);
    }
}