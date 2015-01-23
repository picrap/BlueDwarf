// This is the blue dwarf
// more information at https://code.google.com/p/blue-dwarf/

namespace BlueDwarf.Net.Proxy.Scanner
{
    using Client;

    public interface IProxyValidator
    {
        /// <summary>
        /// Validates the specified proxy host port as a HTTP CONNECT proxy.
        /// </summary>
        /// <param name="proxyHostPort">The proxy host port.</param>
        /// <param name="targetHost">The target host.</param>
        /// <param name="targetPort">The target port.</param>
        /// <param name="routeToProxy">The route to proxy.</param>
        /// <returns></returns>
        bool Validate(HostPort proxyHostPort, string targetHost, int targetPort, ProxyRoute routeToProxy);
    }
}
