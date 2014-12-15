
using BlueDwarf.Annotations;
using BlueDwarf.Net.Name;
using BlueDwarf.Net.Proxy.Client;
using Microsoft.Practices.Unity;
using Org.Mentalis.Proxy.Socks;

namespace BlueDwarf.Net.Proxy.Server
{
    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    public class SocksProxyServer : IProxyServer
    {
        private readonly SocksListener _server;

        [Dependency]
        public IProxyClient ProxyClient { get; set; }

        [Dependency]
        public INameResolver NameResolver { get; set; }

        public ProxyRoute ProxyRoute
        {
            get { return _server.ProxyRoute; }
            set { _server.ProxyRoute = value; }
        }

        public SocksProxyServer()
        {
            _server = new SocksListener(1080);
        }

        public void Start()
        {
            _server.ProxyClient = ProxyClient;
            _server.NameResolver = NameResolver;
            _server.Start();
        }
    }
}
