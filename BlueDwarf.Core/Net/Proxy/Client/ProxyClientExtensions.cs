// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Net.Proxy.Client
{
    using System;

    public static class ProxyClientExtensions
    {
        /// <summary>
        /// Creates a route, but does not throw exceptions.
        /// </summary>
        /// <param name="proxyClient">The proxy client.</param>
        /// <param name="targetHost">The target host.</param>
        /// <param name="targetPort">The target port.</param>
        /// <param name="proxyServers">The proxy servers.</param>
        /// <returns></returns>
        public static ProxyRoute SafeCreateRoute(this IProxyClient proxyClient, string targetHost, int targetPort, params Uri[] proxyServers)
        {
            try
            {
                return proxyClient.CreateRoute(targetHost, targetPort, proxyServers);
            }
            catch (ProxyRouteException)
            { }
            return null;
        }
    }
}
