// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Net.Proxy.Scanner
{
    using System;
    using Client;

    /// <summary>
    /// Proxy validation
    /// </summary>
    public interface IProxyValidator
    {
        /// <summary>
        /// Validates the specified proxy host port as a HTTP CONNECT proxy.
        /// Given a route to get to the proxy and a test target to be reached after proxy
        /// </summary>
        /// <param name="proxyHostPort">The proxy host port.</param>
        /// <param name="testTargetHost"></param>
        /// <param name="testTargetPort"></param>
        /// <param name="route"></param>
        /// <returns></returns>
        bool ValidateHttpConnect(HostPort proxyHostPort, string testTargetHost, int testTargetPort, Route route);
    }
}
