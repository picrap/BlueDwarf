// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Net.Name
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
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
        public IPAddress Resolve(string name, Route route)
        {
            try
            {
                var address = Dns.GetHostAddresses(name).FirstOrDefault();
                return address;
            }
            catch (ArgumentException)
            { }
            catch (SocketException)
            { }
            return null;
        }
    }
}