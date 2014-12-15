﻿using System;
using System.Linq;
using BlueDwarf.Net.Proxy.Server;

namespace BlueDwarf.Net.Proxy.Client
{
    /// <summary>
    /// Represents a route to target
    /// This class holds the router instance
    /// </summary>
    public class ProxyRoute
    {
        public delegate ProxyStream ConnectDelegate(string targetHost, int targetPort, ProxyRoute proxyRoute, bool overrideDns);

        public Uri[] Route { get; private set; }

        private readonly ConnectDelegate _connect;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProxyRoute"/> class.
        /// </summary>
        /// <param name="connect">The connect.</param>
        /// <param name="route">The route.</param>
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

        /// <summary>
        /// Adds an element to current route and returns a new route.
        /// </summary>
        /// <param name="proxyRoute">The proxy route.</param>
        /// <param name="uri">The URI.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
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