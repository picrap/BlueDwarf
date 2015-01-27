// This is the blue dwarf
// more information at https://code.google.com/p/blue-dwarf/

namespace BlueDwarf.Net.Proxy.Scanner
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text.RegularExpressions;
    using Annotations;
    using Utility;

    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    internal class HostScanner : IHostScanner
    {
        internal const string AfterDigitEx = @"(\D|$)";

        // A host here is an IP separated with a colon and optional spaces
        internal const string IPColonPortEx = @"(" + HostPort.IPv4Ex + @"\s*\:\s*" + HostPort.PortEx + AfterDigitEx + ")";
        // In some cases, the port my follow the IP without colon
        internal const string IPSpacePortEx = @"(" + HostPort.IPv4Ex + @"\s+" + HostPort.PortEx + AfterDigitEx + ")";

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
            var hostPortRegex = new Regex(hostPortEx, RegexOptions.Singleline);
            return hostPortRegex.Matches(pageText).Cast<Match>().SelectNonNull(CreateHostPort);
        }

        private static HostPort CreateHostPort(Match match)
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
