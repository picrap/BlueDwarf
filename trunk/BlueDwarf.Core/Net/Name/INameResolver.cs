using System.Net;
using BlueDwarf.Net.Proxy;
using BlueDwarf.Net.Proxy.Client;

namespace BlueDwarf.Net.Name
{
    public interface INameResolver
    {
        IPAddress Resolve(string name, IProxyClient proxyClient, ProxyRoute route);
    }
}