// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Net.Name.StatDns
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using Annotations;
    using Client;
    using Proxy.Client;

    /// <summary>
    /// DNS resolution using the excellent statdns.com resolution
    /// </summary>
    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    internal class StatDnsNameResolver : INameResolver
    {
        private readonly IDictionary<string, Tuple<IPAddress, DateTime>> _entries = new Dictionary<string, Tuple<IPAddress, DateTime>>();

        /// <summary>
        /// Resolves the specified name using the given route.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="route">The route.</param>
        /// <returns></returns>
        public IPAddress Resolve(string name, Route route)
        {
            IPAddress address;
            if (IPAddress.TryParse(name, out address))
                return address;

            var now = DateTime.UtcNow;
            lock (_entries)
            {
                Tuple<IPAddress, DateTime> entry;
                if (_entries.TryGetValue(name, out entry))
                {
                    if (entry.Item2 > now)
                        return entry.Item1;
                }

                var resolvedAddress = RequestionResolution(name, route);
                if (resolvedAddress == null)
                    return null;
                entry = Tuple.Create(resolvedAddress.Item1, now + TimeSpan.FromSeconds(resolvedAddress.Item2));
                _entries[name] = entry;
                return entry.Item1;
            }
        }

        /// <summary>
        /// Requestions the resolution.
        /// This can be done by following multiple resolutions (since CNAME may lead to other CNAME or A)
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="route">The route.</param>
        /// <returns></returns>
        private static Tuple<IPAddress, int> RequestionResolution(string name, Route route)
        {
            for (int hop = 0; hop < 100; hop++)
            {
                var answer = Ask(name, "A", route) ?? Ask(name, "CNAME", route);
                if (answer == null)
                    return null;

                IPAddress address;
                if (IPAddress.TryParse(answer.RData, out address))
                    return Tuple.Create(address, answer.TTL);

                name = answer.RData;
            }
            return null;
        }

        /// <summary>
        /// Query to .
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <param name="route">The route.</param>
        /// <returns></returns>
        private static Answer Ask(string name, string type, Route route)
        {
            try
            {
                var client = Rest.Client<IStatDns>(route);
                var response = client.Ask(name, type);
                if (response.Answers == null)
                    return null;
                return response.Answers[0];
            }
            // TODO: something better here
            catch { }
            return null;
        }
    }
}