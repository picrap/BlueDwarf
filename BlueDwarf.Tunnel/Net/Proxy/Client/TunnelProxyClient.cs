// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Net.Proxy.Client
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using Tunnel.Annotations;

    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    public partial class TunnelProxyClient : IProxyClient
    {
        /// <summary>
        /// Creates a new route.
        /// </summary>
        /// <param name="proxyServers">The proxy servers.</param>
        /// <returns></returns>
        public Route CreateRoute(params ProxyServer[] proxyServers)
        {
            return new Route(TunnelConnectAddress, proxyServers);
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
        private Socket TunnelConnectAddress(IPAddress target, int targetPort, Route route)
        {
            var socket = TunnelConnect(route);
            var newSocket = ConnectSocket(socket, new IPEndPoint(target, targetPort));
            if (newSocket == null)
                throw new ProxyRouteException(target.ToString());

            return newSocket;
        }

        /// <summary>
        /// Tunnel connection using given route.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <returns></returns>
        /// <exception cref="ProxyRouteException"></exception>
        private Tuple<Socket, ProxyProtocol> TunnelConnect(Route route)
        {
            Tuple<Socket, ProxyProtocol> socket = null;
            var routeUntilHere = new Route(TunnelConnectAddress);
            foreach (var proxyServer in route.Relays)
            {
                if (proxyServer == null)
                    continue;

                socket = ConnectProxy(socket, proxyServer);
                routeUntilHere += proxyServer;
                if (socket == null)
                    throw new ProxyRouteException(proxyServer);
            }
            return socket;
        }

        /// <summary>
        /// Connects to the specified server.
        /// </summary>
        /// <param name="socket">The socket.</param>
        /// <param name="proxyServer">The proxy server.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">Unknown proxy type</exception>
        private Tuple<Socket, ProxyProtocol> ConnectProxy(Tuple<Socket, ProxyProtocol> socket, ProxyServer proxyServer)
        {
            var newSocket = ConnectSocket(socket, proxyServer.IPEndPoint);
            if (newSocket == null)
                return null;
            return new Tuple<Socket, ProxyProtocol>(newSocket, proxyServer.Protocol);
        }

        /// <summary>
        /// Single step connect, using a proxy type+target+port.
        /// </summary>
        /// <param name="socket">The stream.</param>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        private Socket ConnectSocket(Tuple<Socket, ProxyProtocol> socket, IPEndPoint target)
        {
            if (socket == null)
                return DirectConnect(target);
            switch (socket.Item2)
            {
                case ProxyProtocol.HttpConnect:
                    return HttpProxyConnect(socket.Item1, target);
                case ProxyProtocol.Socks4A:
                    return SocksProxyConnect(socket.Item1, target);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Direct connection to a given host.
        /// </summary>
        /// <param name="target">The ip end point.</param>
        /// <returns></returns>
        private static Socket DirectConnect(IPEndPoint target)
        {
            var newStream = Connect.To(target.Address, target.Port);
            return newStream;
        }
    }
}
