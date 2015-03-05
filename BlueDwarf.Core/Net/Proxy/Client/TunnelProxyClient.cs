// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Net.Proxy.Client
{
    using System;
    using System.Net;
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
        /// <param name="proxyServers">The proxy servers.</param>
        /// <returns></returns>
        public Route CreateRoute(params Uri[] proxyServers)
        {
            return new Route(TunnelConnectHost, TunnelConnectAddress, proxyServers);
        }

        /// <summary>
        /// Connects the specified target host+port.
        /// No exception handling is done here
        /// </summary>
        /// <param name="targetHost">The target host.</param>
        /// <param name="targetPort">The target port.</param>
        /// <param name="route">The route.</param>
        /// <returns></returns>
        private SocketStream TunnelConnectHost(string targetHost, int targetPort, Route route)
        {
            var stream = TunnelConnect(route);
            var newStream = Connect(stream, targetHost, targetPort, route);

            if (newStream == null)
                throw new ProxyRouteException(targetHost);

            return newStream;
        }

        /// <summary>
        /// Connects the specified target address+port.
        /// No exception handling is done here
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="targetPort">The target port.</param>
        /// <param name="route">The route.</param>
        /// <returns></returns>
        /// <exception cref="ProxyRouteException">
        /// </exception>
        private SocketStream TunnelConnectAddress(IPAddress target, int targetPort, Route route)
        {
            var stream = TunnelConnect(route);
            var newStream = Connect(stream, target, targetPort);

            if (newStream == null)
                throw new ProxyRouteException(target.ToString());

            return newStream;
        }

        /// <summary>
        /// Tunnel connection using given route.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <returns></returns>
        /// <exception cref="ProxyRouteException"></exception>
        private Tuple<SocketStream, ProxyType> TunnelConnect(Route route)
        {
            Tuple<SocketStream, ProxyType> stream = null;
            var routeUntilHere = new Route(TunnelConnectHost, TunnelConnectAddress);
            foreach (var uri in route.Relays)
            {
                if (uri == null)
                    continue;

                IPAddress target;
                if (!IPAddress.TryParse(uri.Host, out target))
                {
                    target = NameResolver.Resolve(uri.Host, routeUntilHere);
                    if (target == null)
                        throw new ProxyRouteException(uri);
                }

                stream = Connect(stream, uri.Scheme, target, uri.Port);
                routeUntilHere += uri;
                if (stream == null)
                    throw new ProxyRouteException(uri);
            }
            return stream;
        }

        /// <summary>
        /// Connects to the specified server.
        /// </summary>
        /// <param name="stream">The stream and proxy type pair.</param>
        /// <param name="scheme">The scheme.</param>
        /// <param name="target">The target.</param>
        /// <param name="targetPort">The target port.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">Unknown proxy type</exception>
        private Tuple<SocketStream, ProxyType> Connect(Tuple<SocketStream, ProxyType> stream, string scheme, IPAddress target, int targetPort)
        {
            var newStream = Connect(stream, target, targetPort);
            if (newStream == null)
                return null;
            if (scheme == Uri.UriSchemeHttp)
                return Tuple.Create(newStream, ProxyType.HttpConnect);
            if (scheme == "socks")
                return Tuple.Create(newStream, ProxyType.Socks4);
            throw new ArgumentException(@"Unknown proxy type", scheme);
        }

        /// <summary>
        /// Connect to a given host using an already existing stream (coming from connection to other proxy servers).
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="targetHost">The target host.</param>
        /// <param name="targetPort">The target port.</param>
        /// <param name="routeUntilHere">The route until here.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        private SocketStream Connect(Tuple<SocketStream, ProxyType> stream, string targetHost, int targetPort, Route routeUntilHere)
        {
            IPAddress target;
            if (!IPAddress.TryParse(targetHost, out target))
            {
                target = NameResolver.Resolve(targetHost, routeUntilHere);
                if (target == null)
                    throw new ProxyRouteException(targetHost);
            }

            return Connect(stream, target, targetPort);
        }

        /// <summary>
        /// Single step connect, using a proxy type+target+port.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="target">The target.</param>
        /// <param name="targetPort">The target port.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        private SocketStream Connect(Tuple<SocketStream, ProxyType> stream, IPAddress target, int targetPort)
        {
            if (stream == null)
                return DirectConnect(null, target, targetPort);
            switch (stream.Item2)
            {
                case ProxyType.Direct:
                    return DirectConnect(stream.Item1, target, targetPort);
                case ProxyType.HttpConnect:
                    return HttpProxyConnect(stream.Item1, target, targetPort);
                case ProxyType.Socks4:
                    return SocksProxyConnect(stream.Item1, target, targetPort);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Direct connection to a given host.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="target">The target.</param>
        /// <param name="targetPort">The target port.</param>
        /// <returns></returns>
        private SocketStream DirectConnect(SocketStream stream, IPAddress target, int targetPort)
        {
            var newStream = Net.Connect.To(target, targetPort);
            return newStream;
        }
    }
}
