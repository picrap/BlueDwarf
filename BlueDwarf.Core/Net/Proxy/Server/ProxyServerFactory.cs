// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Net.Proxy.Server
{
    using Annotations;
    using Microsoft.Practices.Unity;
    using Name;

    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    public class ProxyServerFactory : IProxyServerFactory
    {
        [Dependency]
        public INameResolver NameResolver { get; set; }

        /// <summary>
        /// Creates a socks proxy server.
        /// </summary>
        /// <returns></returns>
        public IProxyServer CreateSocksProxyServer()
        {
            return new SocksProxyServer(NameResolver);
        }
    }
}
