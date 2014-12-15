using System.Linq;
using System.Net;
using BlueDwarf.Annotations;
using BlueDwarf.Net.Proxy.Client;

namespace BlueDwarf.Net.Name
{
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