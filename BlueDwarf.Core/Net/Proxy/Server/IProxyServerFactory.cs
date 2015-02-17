// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Net.Proxy.Server
{
    /// <summary>
    /// Proxy server factory interface
    /// (since we may create several proxy servers at the same time)
    /// </summary>
    public interface IProxyServerFactory
    {
        /// <summary>
        /// Creates a socks proxy server.
        /// </summary>
        /// <returns></returns>
        IProxyServer CreateSocksProxyServer();
    }
}