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
        /// <param name="proxyServer"></param>
        /// <param name="testTarget"></param>
        /// <param name="routeToProxy"></param>
        /// <param name="tryCount">The try count to fully validate the route</param>
        /// <returns></returns>
        bool Validate(ProxyServer proxyServer, Uri testTarget, Route routeToProxy, int tryCount = 3);

        /// <summary>
        /// Validates the specified route, using the test target.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <param name="testTarget">The test target.</param>
        /// <param name="tryCount">The try count to fully validate the route</param>
        /// <returns></returns>
        void Validate(Route route, Uri testTarget, int tryCount = 3);
    }
}
