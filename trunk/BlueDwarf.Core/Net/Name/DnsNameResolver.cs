using System.Linq;
using System.Net;
using BlueDwarf.Net.Proxy.Client;

namespace BlueDwarf.Net.Name
{
    public class DnsNameResolver : INameResolver
    {
        public IPAddress Resolve(string name, IProxyClient proxyClient, ProxyRoute route)
        {
            var address = Dns.GetHostAddresses(name).FirstOrDefault();
            return address;
        }
    }
}