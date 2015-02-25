using BlueDwarf.Net.Proxy.Client;
using Fiddler;

namespace BlueDwarf.Net.Proxy.Server
{
    public class HttpProxyServer : IProxyServer
    {
        public ProxyRoute ProxyRoute { get; set; }

        public void Start()
        {
            FiddlerApplication.Startup(3128,FiddlerCoreStartupFlags.Default);
            var proxy = FiddlerApplication.oProxy;
            FiddlerApplication.BeforeRequest += OnBeforeRequest;
        }

        private void OnBeforeRequest(Session session)
        {
        }
    }
}
