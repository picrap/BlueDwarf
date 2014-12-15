using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using BlueDwarf.Annotations;
using BlueDwarf.Net.Name;
using BlueDwarf.Net.Proxy.Server;
using Microsoft.Practices.Unity;

namespace BlueDwarf.Net.Proxy.Client
{
    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    public partial class TunnelProxyClient : IProxyClient
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
        /// <param name="testTarget">The test target.</param>
        /// <param name="proxyServers">The proxy servers.</param>
        /// <returns></returns>
        public ProxyRoute CreateRoute(Uri testTarget, params Uri[] proxyServers)
        {
            try
            {
                if (testTarget == null)
                    return null;

                var proxyRoute = new ProxyRoute(Connect, proxyServers);
                var stream = Connect(testTarget.Host, testTarget.Port, proxyRoute, false);

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
        private ProxyStream Connect(string targetHost, int targetPort, ProxyRoute route, bool overrideDns)
        {
            Tuple<ProxyStream, ProxyType> stream = null;
            var routeUntilHere = new ProxyRoute(Connect);
            foreach (var uri in route.Route)
            {
                if (uri == null)
                    continue;
                stream = Connect(stream, uri, routeUntilHere, overrideDns);
                routeUntilHere += uri;
                if (stream == null)
                    return null;
            }
            return Connect(stream, targetHost, targetPort, route, overrideDns);
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
        private Tuple<ProxyStream, ProxyType> Connect(Tuple<ProxyStream, ProxyType> stream, Uri server, ProxyRoute route, bool overrideDns)
        {
            var newStream = Connect(stream, server.Host, server.Port, route, overrideDns);
            if (newStream == null)
                return null;
            if (server.Scheme == Uri.UriSchemeHttp)
                return Tuple.Create(newStream, ProxyType.HttpConnect);
            if (server.Scheme == "socks")
                return Tuple.Create(newStream, ProxyType.Socks4);
            throw new ArgumentException(@"Unknown proxy type", server.ToString());
        }

        private ProxyStream Connect(Tuple<ProxyStream, ProxyType> stream, string targetHost, int targetPort, ProxyRoute routeUntilHere, bool overrideDns)
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
                    return HttpProxyConnect(stream.Item1, targetHost, targetPort, routeUntilHere);
                case ProxyType.Socks4:
                    return SocksProxyConnect(stream.Item1, targetHost, targetPort, routeUntilHere);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private ProxyStream DirectConnect(ProxyStream stream, string targetHost, int targetPort, ProxyRoute routeUntilHere)
        {
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    socket.Connect(targetHost, targetPort);
                    var newStream = new ProxyStream(socket, true);
                    return newStream;
                }
                catch (SocketException)
                {
                }
                catch (IOException)
                {
                }
                Thread.Sleep(1000);
            }
            return null;
        }
    }
}
