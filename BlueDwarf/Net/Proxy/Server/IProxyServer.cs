using BlueDwarf.Net.Proxy.Client;

namespace BlueDwarf.Net.Proxy.Server
{
    public interface IProxyServer
    {
        ProxyRoute ProxyRoute { get; set; }

        void Start();
    }
}