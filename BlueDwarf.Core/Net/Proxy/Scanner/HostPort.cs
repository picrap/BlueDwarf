// This is the blue dwarf
// more information at https://code.google.com/p/blue-dwarf/

namespace BlueDwarf.Net.Proxy.Scanner
{
    using System;
    using System.Diagnostics;
    using System.Net;
    using Utility;

    [DebuggerDisplay("{HostOrAddress}:{Port}")]
    public class HostPort : IEquatable<HostPort>
    {
        // A literal decimal byte, from 0 to 255
        private const string ByteRangeEx = @"(\d{1,2}|([0-1]\d{2})|(2[0-4]\d)|(25[0-5]))";
        // so an IP v4 is 4 literal bytes, separated by a dot
        public const string IPv4Ex = @"(?<address>(" + ByteRangeEx + @"\." + ByteRangeEx + @"\." + ByteRangeEx + @"\." + ByteRangeEx + @"))";
        // A port from 0 to 65535
        public const string PortEx = @"(?<port>(([0-5]\d{4})|(6[0-4]\d{3})|(65[0-4]\d{2})|(655[0-2]\d)|(6553[0-5])|\d{1,4}))";

        /// <summary>
        /// Gets the host or address (currently for debugging only).
        /// </summary>
        /// <value>
        /// The host or address.
        /// </value>
        public string HostOrAddress { get { return Host ?? Address.ToString(); } }

        /// <summary>
        /// Gets the host, or null if Address is not null.
        /// </summary>
        /// <value>
        /// The host.
        /// </value>
        public string Host { get; private set; }
        /// <summary>
        /// Gets the address, or null if Host is not null.
        /// </summary>
        /// <value>
        /// The address.
        /// </value>
        public IPAddress Address { get; private set; }

        /// <summary>
        /// Gets the port.
        /// </summary>
        /// <value>
        /// The port.
        /// </value>
        public int Port { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HostPort"/> class.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="port">The port.</param>
        public HostPort(string host, int port)
        {
            Host = host;
            Port = port;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HostPort"/> class.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="port">The port.</param>
        public HostPort(IPAddress address, int port)
        {
            Address = address;
            Port = port;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        public bool Equals(HostPort other)
        {
            return other != null
                   && Host.SafeEquals(other.Host)
                   && Address.SafeEquals(other.Address)
                   && Port.SafeEquals(other.Port);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as HostPort);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            int hash = Port;
            if (Host != null)
                hash ^= Host.GetHashCode();
            if (Address != null)
                hash ^= Address.GetHashCode();
            return hash;
        }
    }
}
