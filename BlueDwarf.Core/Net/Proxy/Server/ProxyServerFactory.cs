// This is the blue dwarf
// more information at https://code.google.com/p/blue-dwarf/

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

        public IProxyServer CreateSocksProxyServer()
        {
            return new SocksProxyServer(NameResolver);
        }
    }
}
