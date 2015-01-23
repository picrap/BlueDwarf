// This is the blue dwarf
// more information at https://code.google.com/p/blue-dwarf/

namespace BlueDwarf.Net.Proxy.Scanner
{
    using System;
    using System.Net;
    using Utility;

    public class HostPort : IEquatable<HostPort>
    {
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
