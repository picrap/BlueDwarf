// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf

namespace BlueDwarf.Net.Proxy.Client
{
    using System;
    using System.IO;
    using System.Net.Sockets;
    using Name;
    using Utility;

    /// <summary>
    /// Extensions to <see cref="Route"/>
    /// </summary>
    public static class RouteExtensions
    {
        /// <summary>
        /// Connects to specified target host/port.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <param name="targetHost">The target host.</param>
        /// <param name="targetPort">The target port.</param>
        /// <param name="nameResolver">The name resolver.</param>
        /// <returns></returns>
        public static Socket Connect(this Route route, string targetHost, int targetPort, INameResolver nameResolver)
        {
            var targetAddress = nameResolver.Resolve(targetHost, route);
            if (targetAddress == null)
                throw new ProxyRouteException(targetHost);
            return route.Connect(targetAddress, targetPort);
        }

        /// <summary>
        /// Connects the specified route.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <param name="uri">The URI.</param>
        /// <param name="nameResolver">The name resolver.</param>
        /// <returns></returns>
        public static Stream Connect(this Route route, Uri uri, INameResolver nameResolver)
        {
            Stream stream = Connect(route, uri.Host, uri.Port, nameResolver).ToNetworkStream();
            try
            {
                if (stream != null && string.Equals(uri.Scheme, Uri.UriSchemeHttps, StringComparison.InvariantCultureIgnoreCase))
                    stream = stream.AsSsl(uri.Host);
            }
            catch (IOException)
            {
                throw new ProxyRouteException(uri.Host);
            }
            return stream;
        }

    }
}
