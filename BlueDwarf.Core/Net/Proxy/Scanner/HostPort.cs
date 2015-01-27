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

        public string HostOrAddress { get { return Host ?? Address.ToString(); } }

        public string Host { get; private set; }
        public IPAddress Address { get; private set; }

        public int Port { get; private set; }

        public HostPort(string host, int port)
        {
            Host = host;
            Port = port;
        }

        public HostPort(IPAddress address, int port)
        {
            Address = address;
            Port = port;
        }

        public bool Equals(HostPort other)
        {
            return other != null
                   && Host.SafeEquals(other.Host)
                   && Address.SafeEquals(other.Address)
                   && Port.SafeEquals(other.Port);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as HostPort);
        }

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
