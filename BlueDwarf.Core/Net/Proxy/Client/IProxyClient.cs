using System;

namespace BlueDwarf.Net.Proxy.Client
{
    public interface IProxyClient
    {
        /// <summary>
        /// Occurs when a new connection to target is made.
        /// </summary>
        event EventHandler<ProxyClientConnectEventArgs> Connect;

        /// <summary>
        /// Occurs when a transfer is made.
        /// </summary>
        event EventHandler<ProxyClientTransferEventArgs> Transfer;

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