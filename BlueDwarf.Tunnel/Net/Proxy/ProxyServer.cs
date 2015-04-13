// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Net.Proxy
{
    using System;
    using System.Diagnostics;
    using System.Net;

    /// <summary>
    /// Represents a proxy server
    /// </summary>
    [DebuggerDisplay("{Literal}")]
    public partial class ProxyServer : HostEndPoint
    {
        /// <summary>
        /// Gets the protocol.
        /// </summary>
        /// <value>
        /// The protocol.
        /// </value>
        public ProxyProtocol Protocol { get; private set; }

        /// <summary>
        /// Gets the URI.
        /// </summary>
        /// <value>
        /// The URI.
        /// </value>
        public Uri Uri
        {
            get { return new Uri(string.Format("{0}://{1}:{2}", GetLiteralProtocol(Protocol), Host, Port)); }
        }

        /// <summary>
        /// Gets the literal in form protocol=host:port
        /// </summary>
        /// <value>
        /// The literal.
        /// </value>
        public string Literal
        {
            get { return string.Format("{0}={1}:{2}", Protocol, Host, Port); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProxyServer"/> class.
        /// </summary>
        /// <param name="protocol">The protocol.</param>
        /// <param name="address">The address.</param>
        /// <param name="port">The port.</param>
        public ProxyServer(ProxyProtocol protocol, IPAddress address, int port)
            : base(address, port)
        {
            Protocol = protocol;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProxyServer"/> class.
        /// </summary>
        /// <param name="protocol">The protocol.</param>
        /// <param name="host">The host.</param>
        /// <param name="port">The port.</param>
        public ProxyServer(ProxyProtocol protocol, string host, int port)
            : base(host, port)
        {
            Protocol = protocol;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProxyServer"/> class.
        /// </summary>
        /// <param name="uri">The URI.</param>
        public ProxyServer(Uri uri)
            : base(uri.Host, GetPort(uri))
        {
            Protocol = GetProtocol(uri.Scheme);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProxyServer"/> class.
        /// </summary>
        /// <param name="literalValue">The literal value.</param>
        public ProxyServer(string literalValue)
            : base(GetHostFromLiteral(literalValue), GetPortFromLiteral(literalValue))
        {
            Protocol = GetProtocolFromLiteral(literalValue);
        }

        /// <summary>
        /// Gets the port.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>
        private static int GetPort(Uri uri)
        {
            var port = uri.Port;
            if (port > 0)
                return port;
            if (uri.Scheme == "socks")
                return 1080;
            return 0;
        }

        /// <summary>
        /// Gets the literal protocol.
        /// </summary>
        /// <param name="proxyProtocol">The proxy protocol.</param>
        /// <returns></returns>
        private static string GetLiteralProtocol(ProxyProtocol proxyProtocol)
        {
            switch (proxyProtocol)
            {
                case ProxyProtocol.HttpConnect:
                    return "http";
                case ProxyProtocol.Socks4A:
                    return "socks";
                default:
                    throw new ArgumentOutOfRangeException("proxyProtocol");
            }
        }

        /// <summary>
        /// Gets the protocol.
        /// </summary>
        /// <param name="scheme">The scheme.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        private static ProxyProtocol GetProtocol(string scheme)
        {
            switch (scheme)
            {
                case "http":
                case "https":
                    return ProxyProtocol.HttpConnect;
                case "socks":
                case "socks4":
                    return ProxyProtocol.Socks4A;
                default:
                    throw new ArgumentOutOfRangeException(scheme);
            }
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Uri"/> to <see cref="ProxyServer"/>.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator ProxyServer(Uri uri)
        {
            if (uri == null)
                return null;
            return new ProxyServer(uri);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.String"/> to <see cref="ProxyServer"/>.
        /// </summary>
        /// <param name="literal">The literal.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator ProxyServer(string literal)
        {
            if (literal == null)
                return null;
            return new ProxyServer(literal);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="comparand">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object comparand)
        {
            var other = comparand as ProxyServer;
            if (other == null)
                return false;
            return Host == other.Host && Port == other.Port && Protocol == other.Protocol;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        /// </PermissionSet>
        public override int GetHashCode()
        {
            return Host.GetHashCode() ^ Port.GetHashCode() ^ Protocol.GetHashCode();
        }
    }
}
