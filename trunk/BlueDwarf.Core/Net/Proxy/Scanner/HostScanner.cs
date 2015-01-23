
namespace BlueDwarf.Net.Proxy.Scanner
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text.RegularExpressions;
    using Utility;

    internal class HostScanner : IHostScanner
    {
        // A literal decimal byte, from 0 to 255
        private const string ByteRangeEx = @"(\d{1,2}|([0-1]\d{2})|(2[0-4]\d)|(25[0-5]))";
        // so an IP v4 is 4 literal bytes, separated by a dot
        internal const string IPv4Ex = @"(?<address>(" + ByteRangeEx + @"\." + ByteRangeEx + @"\." + ByteRangeEx + @"\." + ByteRangeEx + @"))";
        // A port from 0 to 65535
        internal const string PortRangeEx = @"(?<port>(([0-5]\d{4})|(6[0-4]\d{3})|(65[0-4]\d{2})|(655[0-2]\d)|(6553[0-5])|\d{1,4}))";
        internal const string AfterDigitEx = @"(\D|$)";

        // A host here is an IP separated with a colon and optional spaces
        internal const string IPColonPortEx = @"(" + IPv4Ex + @"\s*\:\s*" + PortRangeEx + AfterDigitEx + ")";
        // In some cases, the port my follow the IP without colon
        internal const string IPSpacePortEx = @"(" + IPv4Ex + @"\s+" + PortRangeEx + AfterDigitEx + ")";

        private const string AnyEx = IPColonPortEx + "|" + IPSpacePortEx;

        /// <summary>
        /// Scans the specified page text.
        /// </summary>
        /// <param name="pageText">The page text.</param>
        /// <param name="hostPortEx">The host port regular expression, or null to use internal capture.</param>
        /// <returns></returns>
        public IEnumerable<HostPort> Scan(string pageText, string hostPortEx = null)
        {
            return CreateHostPorts(pageText, hostPortEx ?? AnyEx);
        }

        internal static IEnumerable<HostPort> CreateHostPorts(string pageText, string hostPortEx)
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
    }
}
