
using BlueDwarf.Annotations;
using BlueDwarf.Net.Name;
using BlueDwarf.Net.Proxy.Client;
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
        private int _port = 0;

        [Dependency]
        public IProxyClient ProxyClient { get; set; }

        [Dependency]
        public INameResolver NameResolver { get; set; }

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
                _server = new SocksListener(Port) { ProxyClient = ProxyClient, NameResolver = NameResolver, ProxyRoute = _proxyRoute };
                _server.Start();
            }
        }
    }
}
