// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Net.Proxy.Server
{
    using System;
    using System.Net.Sockets;
    using Client;
    using Name;
    using Org.Mentalis.Proxy.Socks;
    using Utility;

    /// <summary>
    /// Socks proxy server
    /// Was from far the easiest to implement
    /// </summary>
    internal class SocksProxyServer : IProxyServer
    {
        private SocksListener _server;

        private readonly INameResolver _nameResolver;

        /// <summary>
        /// Occurs when a client connects.
        /// </summary>
        public event EventHandler Connect;
        /// <summary>
        /// Occurs when data is transferred, from or to client.
        /// </summary>
        public event EventHandler<ProxyServerTransferEventArgs> Transfer;

        private int? _port;
        public int? Port
        {
            get { return _port; }
            set
            {
                if (_port != value)
                {
                    _port = value;
                    ConfigureServer();
                }
            }
        }

        private ProxyRoute _proxyRoute;

        public SocksProxyServer(INameResolver nameResolver)
        {
            _nameResolver = nameResolver;
        }

        /// <summary>
        /// Disposes the instance.
        /// </summary>
        public void Dispose()
        {
            // By setting the port to null, we release the current server (if it exists) and do not start another one
            Port = null;
        }

        /// <summary>
        /// Gets or sets the proxy route.
        /// </summary>
        /// <value>
        /// The proxy route.
        /// </value>
        public ProxyRoute ProxyRoute
        {
            get { return _proxyRoute; }
            set
            {
                if (_server != null)
                    _server.ProxyRoute = value;
                _proxyRoute = value;
            }
        }

        private void ConfigureServer()
        {
            if (_server != null)
            {
                _server.Dispose();
                _server = null;
            }
            if (Port.HasValue)
            {
                if (Port.Value > 0)
                {
                    _server = CreateListener(Port.Value);
                }
                else
                {
                    for (; ; )
                    {
                        try
                        {
                            _server = CreateListener(0);
                            _port = _server.Port;
                            break;
                        }
                        catch (SocketException)
                        {
                        }
                    }
                }
            }
        }

        private SocksListener CreateListener(int port)
        {
            var server = new SocksListener(port) { NameResolver = _nameResolver, ProxyRoute = _proxyRoute };
            server.ClientConnected += OnClientConnected;
            server.ClientReceived += OnClientReceived;
            server.RemoteReceived += OnRemoteReceived;
            server.Start();
            return server;
        }

        private void OnClientConnected(object sender, EventArgs e)
        {
            Connect.Raise(this);
        }

        private void OnRemoteReceived(object sender, ClientReceivedEventArgs e)
        {
            Transfer.Raise(this, new ProxyServerTransferEventArgs(e.Received, 0));
        }

        private void OnClientReceived(object sender, ClientReceivedEventArgs e)
        {
            Transfer.Raise(this, new ProxyServerTransferEventArgs(0, e.Received));
        }
    }
}
