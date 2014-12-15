using BlueDwarf.Net.Proxy.Client;
using Microsoft.Practices.Unity;

namespace BlueDwarf.Net.Proxy.Server
{
    public class ProxyServer : IProxyServer
    {
        private readonly HttpProxyListener _server;

        [Dependency]
        public IProxyClient ProxyClient { get; set; }

        public ProxyRoute ProxyRoute
        {
            get { return _server.ProxyRoute; }
            set { _server.ProxyRoute = value; }
        }

        public ProxyServer()
        {
            _server = new HttpProxyListener(3128);
        }

        public void Start()
        {
            _server.ProxyClient = ProxyClient;
            _server.Start();
        }
    }
}
