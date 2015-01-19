// This is the blue dwarf
// more information at https://code.google.com/p/blue-dwarf/

namespace BlueDwarf.Net.Name
{
    using System.Linq;
    using System.Net;
    using Annotations;
    using Proxy.Client;

    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    internal class DnsNameResolver : INameResolver
    {
        /// <summary>
        /// Simple DNS resolution using framework.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="route">The route.</param>
        /// <returns></returns>
        public IPAddress Resolve(string name, ProxyRoute route)
        {
            var address = Dns.GetHostAddresses(name).FirstOrDefault();
            return address;
        }
    }
}