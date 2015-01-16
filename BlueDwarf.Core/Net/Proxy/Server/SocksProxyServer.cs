
using System;
using BlueDwarf.Annotations;
using BlueDwarf.Net.Name;
using BlueDwarf.Net.Proxy.Client;
using BlueDwarf.Utility;
using Microsoft.Practices.Unity;
using Org.Mentalis.Proxy.Socks;

namespace BlueDwarf.Net.Proxy.Server
{
    /// <summary>
    /// Socks proxy server
    /// Was from far the easiest to implement
    /// </summary>
    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    public class SocksProxyServer : IProxyServer
    {
        private SocksListener _server;

        [Dependency]
        public INameResolver NameResolver { get; set; }

        /// <summary>
        /// Occurs when a client connects.
        /// </summary>
        public event EventHandler Connect;
        /// <summary>
        /// Occurs when data is transferred, from or to client.
        /// </summary>
        public event EventHandler<ProxyServerTransferEventArgs> Transfer;

        private int _port = 0;
        public int Port
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

        public void Start()
        {
            ConfigureServer();
        }

        private void ConfigureServer()
        {
            if (_server != null)
            {
                _server.Dispose();
                _server = null;
            }
            if (Port > 0)
            {
                _server = new SocksListener(Port) { NameResolver = NameResolver, ProxyRoute = _proxyRoute };
                _server.ClientConnected += OnClientConnected;
                _server.ClientReceived += OnClientReceived;
                _server.RemoteReceived += OnRemoteReceived;
                _server.Start();
            }
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
