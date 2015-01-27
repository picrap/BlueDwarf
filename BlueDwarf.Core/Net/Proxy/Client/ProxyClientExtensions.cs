// This is the blue dwarf
// more information at https://code.google.com/p/blue-dwarf/

namespace BlueDwarf.Net.Proxy.Client
{
    using System;

    public static class ProxyClientExtensions
    {
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
