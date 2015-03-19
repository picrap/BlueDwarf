// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf

namespace BlueDwarf.Net.Name
{
    using System.Linq;
    using System.Net;
    using Microsoft.Practices.Unity;
    using Proxy.Client;
    using StatDns;

    internal class MultiNameResolver : INameResolver
    {
        [Dependency]
        public DnsNameResolver DnsNameResolver { get; set; }

        [Dependency]
        public StatDnsNameResolver StatDnsNameResolver { get; set; }

        /// <summary>
        /// Resolves the specified name using the given route.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="route">The route.</param>
        /// <returns></returns>
        public IPAddress Resolve(string name, Route route)
        {
            return Resolve(name, route, StatDnsNameResolver, DnsNameResolver);
        }

        /// <summary>
        /// Resolves the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="route">The route.</param>
        /// <param name="resolvers">The resolvers.</param>
        /// <returns></returns>
        private static IPAddress Resolve(string name, Route route, params INameResolver[] resolvers)
        {
            return resolvers.Select(r => r.Resolve(name, route)).FirstOrDefault(a => a != null);
        }
    }
}
