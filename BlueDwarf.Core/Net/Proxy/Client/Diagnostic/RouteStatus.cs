// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Net.Proxy.Client.Diagnostic
{
    using System;

    [Flags]
    public enum RouteStatus
    {
        /// <summary>
        /// A proxy is required for this route (if this flag is missing, then we don't need any proxy)
        /// </summary>
        HasProxy = 0x0001,
        /// <summary>
        /// The proxy accepts to connect using DNS
        /// </summary>
        ProxyAcceptsName = 0x0002,
        /// <summary>
        /// The proxy accepts to connect using IP
        /// If this flag is set and not ProxyAcceptsName, then we are behind a proxy filtering by DNS
        /// </summary>
        ProxyAcceptsAddress = 0x0004,
    }
}
