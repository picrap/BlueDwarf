// This is the blue dwarf
// more information at https://code.google.com/p/blue-dwarf/

namespace BlueDwarf.Net.Proxy.Scanner
{
    using System;
    using Client;

    public interface IProxyValidator
    {
        /// <summary>
        /// Validates the specified proxy host port as a HTTP CONNECT proxy.
        /// </summary>
        /// <param name="proxyHostPort">The proxy host port.</param>
        /// <param name="testTargetHost"></param>
        /// <param name="testTargetPort"></param>
        /// <param name="routeToProxy">The route to proxy.</param>
        /// <returns></returns>
        bool Validate(HostPort proxyHostPort, string testTargetHost, int testTargetPort, params Uri[] routeToProxy);
    }
}