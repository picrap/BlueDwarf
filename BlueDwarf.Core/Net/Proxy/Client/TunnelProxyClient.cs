// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Net.Proxy.Client
{
    using System;
    using System.IO;
    using Annotations;
    using Microsoft.Practices.Unity;
    using Name;
    using Server;

    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    internal partial class TunnelProxyClient : IProxyClient
    {
        [Dependency]
        public INameResolver NameResolver { get; set; }

        private enum ProxyType
        {
            Direct,
            HttpConnect,
            Socks4
        }

        /// <summary>
        /// Validates and creates a route.
        /// </summary>
        /// <param name="targetHost"></param>
        /// <param name="targetPort"></param>
        /// <param name="proxyServers">The proxy servers.</param>
        /// <returns></returns>
        public ProxyRoute CreateRoute(string targetHost, int targetPort, params Uri[] proxyServers)
        {
            try
            {
                if (targetHost == null)
                    return null;

                var proxyRoute = new ProxyRoute(TunnelConnect, proxyServers);
                var stream = TunnelConnect(targetHost, targetPort, proxyRoute, true);

                if (stream == null)
                    return null;

                return proxyRoute;
            }
            catch (IOException)
            { }
            catch (ArgumentException)
            { }
            return null;
        }

        /// <summary>
        /// Connects the specified target host+port.
        /// No exception handling is done here
        /// </summary>
        /// <param name="targetHost">The target host.</param>
        /// <param name="targetPort">The target port.</param>
        /// <param name="route">The route.</param>
        /// <param name="overrideDns">if set to <c>true</c> [override DNS].</param>
        /// <returns></returns>
        private ProxyStream TunnelConnect(string targetHost, int targetPort, ProxyRoute route, bool overrideDns)
        {
            Tuple<ProxyStream, ProxyType> stream = null;
            var routeUntilHere = new ProxyRoute(TunnelConnect);
            foreach (var uri in route.Route)
            {
                if (uri == null)
                    continue;
                stream = TunnelConnect(stream, uri, routeUntilHere, overrideDns);
                routeUntilHere += uri;
                if (stream == null)
                    throw new ProxyRouteException(uri);
            }
            var newStream = TunnelConnect(stream, targetHost, targetPort, route, overrideDns);

            if (newStream == null)
                throw new ProxyRouteException(targetHost);

            return newStream;
        }

        /// <summary>
        /// Connects to the specified server.
        /// </summary>
        /// <param name="stream">The stream and proxy type pair.</param>
        /// <param name="server">The server.</param>
        /// <param name="route">The route.</param>
        /// <param name="overrideDns">if set to <c>true</c> [override DNS].</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">@Unknown proxy type</exception>
        private Tuple<ProxyStream, ProxyType> TunnelConnect(Tuple<ProxyStream, ProxyType> stream, Uri server, ProxyRoute route, bool overrideDns)
        {
            var newStream = TunnelConnect(stream, server.Host, server.Port, route, overrideDns);
            if (newStream == null)
                return null;
            if (server.Scheme == Uri.UriSchemeHttp)
                return Tuple.Create(newStream, ProxyType.HttpConnect);
            if (server.Scheme == "socks")
                return Tuple.Create(newStream, ProxyType.Socks4);
            throw new ArgumentException(@"Unknown proxy type", server.ToString());
        }

        /// <summary>
        /// Connect to a given host using an already existing stream (coming from connection to other proxy servers).
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="targetHost">The target host.</param>
        /// <param name="targetPort">The target port.</param>
        /// <param name="routeUntilHere">The route until here.</param>
        /// <param name="overrideDns">if set to <c>true</c> [override DNS].</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        private ProxyStream TunnelConnect(Tuple<ProxyStream, ProxyType> stream, string targetHost, int targetPort, ProxyRoute routeUntilHere, bool overrideDns)
        {
            if (overrideDns)
            {
                var ipAddress = NameResolver.Resolve(targetHost, routeUntilHere);
                targetHost = ipAddress != null ? ipAddress.ToString() : targetHost;
            }
            if (stream == null)
                return DirectConnect(null, targetHost, targetPort, routeUntilHere);
            switch (stream.Item2)
            {
                case ProxyType.Direct:
                    return DirectConnect(stream.Item1, targetHost, targetPort, routeUntilHere);
                case ProxyType.HttpConnect:
                    return HttpProxyConnect(stream.Item1, targetHost, targetPort);
                case ProxyType.Socks4:
                    return SocksProxyConnect(stream.Item1, targetHost, targetPort, routeUntilHere);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Direct connection to a given host.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="targetHost">The target host.</param>
        /// <param name="targetPort">The target port.</param>
        /// <param name="routeUntilHere">The route until here.</param>
        /// <returns></returns>
        private ProxyStream DirectConnect(ProxyStream stream, string targetHost, int targetPort, ProxyRoute routeUntilHere)
        {
            var newStream = Connect.To(targetHost, targetPort);
            return newStream;
        }
    }
}
