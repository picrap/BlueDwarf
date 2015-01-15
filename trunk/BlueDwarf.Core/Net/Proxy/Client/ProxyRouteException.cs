﻿
using System;

namespace BlueDwarf.Net.Proxy.Client
{
    public class ProxyRouteException : Exception
    {
        /// <summary>
        /// Gets or sets the proxy where the connexion error happened.
        /// </summary>
        /// <value>
        /// The proxy.
        /// </value>
        public Uri Proxy { get; set; }

        /// <summary>
        /// If all proxy succeed but connection to target fails, the TargetHost is filled.
        /// </summary>
        /// <value>
        /// The target host.
        /// </value>
        public string TargetHost { get; set; }

        public ProxyRouteException(Uri proxy)
        {
            Proxy = proxy;
        }

        public ProxyRouteException(string targetHost)
        {
            TargetHost = targetHost;
        }
    }
}
