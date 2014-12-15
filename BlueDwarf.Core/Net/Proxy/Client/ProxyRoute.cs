using System;
using System.Linq;
using BlueDwarf.Net.Proxy.Server;

namespace BlueDwarf.Net.Proxy.Client
{
    public class ProxyRoute
    {
        public delegate ProxyStream ConnectDelegate(string targetHost, int targetPort, ProxyRoute proxyRoute, bool overrideDns);

        public Uri[] Route { get; private set; }

        private readonly ConnectDelegate _connect;

        public ProxyRoute(ConnectDelegate connect, params Uri[] route)
        {
            _connect = connect;
            Route = route.Where(r => r != null).ToArray();
        }

        public ProxyRoute GetPrevious()
        {
            if (Route.Length == 0)
                return null;
            return new ProxyRoute(_connect, Route.Take(Route.Length - 1).ToArray());
        }

        public static ProxyRoute operator +(ProxyRoute proxyRoute, Uri uri)
        {
            return new ProxyRoute(proxyRoute._connect, proxyRoute.Route.Concat(new[] { uri }).ToArray());
        }

        /// <summary>
        /// Connects to specified target host/port.
        /// </summary>
        /// <param name="targetHost">The target host.</param>
        /// <param name="targetPort">The target port.</param>
        /// <param name="overrideDns">if set to <c>true</c> resolve DNS by third party before connecting.</param>
        /// <returns></returns>
        public ProxyStream Connect(string targetHost, int targetPort, bool overrideDns)
        {
            return _connect(targetHost, targetPort, this, overrideDns);
        }
    }
}