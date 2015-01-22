
namespace BlueDwarf.Net.Proxy.Scanner
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text.RegularExpressions;
    using Utility;

    internal class HostScanner
    {
        private const string ByteRangeEx = @"(\d{1,2}|([0-1]\d{2})|(2[0-4]\d)|(25[0-5]))";
        internal const string IPv4Ex = @"(?<address>(" + ByteRangeEx + @"\." + ByteRangeEx + @"\." + ByteRangeEx + @"\." + ByteRangeEx + @"))";
        internal const string PortRangeEx = @"(?<port>((\d{1,4})|([0-5]\d{4})|(6[0-4]\d{3})|(65[0-4]\d{2})|(655[0-2]\d)|(6553[0-5])))";

        private const string IPColonPortEx = IPv4Ex + @"\w*\:\w*" + PortRangeEx;
        private const string IPSpacePortEx = IPv4Ex + @"\w+" + PortRangeEx;

        private const string AnyEx = IPColonPortEx + "|" + IPSpacePortEx;

        public IEnumerable<HostPort> Scan(string pageText, string hostPortEx)
        {
            var hostPortRegex = new Regex(hostPortEx);
            return hostPortRegex.Matches(pageText).Cast<Match>().SelectNonNull(CreateHostPort);
        }

        internal static HostPort CreateHostPort(Match match)
        {
            var literalPort = match.Groups["port"].Value;
            int port;
            if (!int.TryParse(literalPort, out port))
                return null;
            var host = match.Groups["host"].Value;
            if (!string.IsNullOrEmpty(host))
                return new HostPort(host, port);

            var literalAddress = match.Groups["address"].Value;
                IPAddress address;
                if (IPAddress.TryParse(literalAddress, out address))
                    return new HostPort(address, port);

            return null;
        }

        public IEnumerable<HostPort> Scan(string pageText)
        {
            return Scan(pageText, AnyEx);
        }
    }
}
