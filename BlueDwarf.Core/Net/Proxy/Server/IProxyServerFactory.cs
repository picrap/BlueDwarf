namespace BlueDwarf.Net.Proxy.Server
{
    public interface IProxyServerFactory
    {
        IProxyServer CreateSocksProxyServer();
    }
}