// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
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
        public IEnumerable<ProxyServer> Scan(string pageText, string hostPortEx = null)
        {
            return CreateHostEndPoints(pageText, hostPortEx ?? AnyEx);
        }

        /// <summary>
        /// Creates the HostPort instances, given a full text and an regex.
        /// </summary>
        /// <param name="pageText">The page text.</param>
        /// <param name="hostPortEx">The host port ex.</param>
        /// <returns></returns>
        internal static IEnumerable<ProxyServer> CreateHostEndPoints(string pageText, string hostPortEx)
        {
            var hostPortRegex = new Regex(hostPortEx, RegexOptions.Singleline);
            return hostPortRegex.Matches(pageText).Cast<Match>().SelectNonNull(CreateProxyServer);
        }

        /// <summary>
        /// Creates a signel HostPort, or null if the match does not give all information.
        /// </summary>
        /// <param name="match">The match.</param>
        /// <returns></returns>
        private static ProxyServer CreateProxyServer(Match match)
        {
            var literalProtocol = match.Groups["protocol"].Value;
            if (literalProtocol.IsNullOrEmpty())
                literalProtocol = "http";
            var proxyProtocol = ProxyProtocolUtility.FromLiteral(literalProtocol);

            var literalPort = match.Groups["port"].Value;
            int port;
            // if port is not provided or invalid, no HostPort is created
            if (!int.TryParse(literalPort, out port))
                return null;
            var host = match.Groups["host"].Value;
            if (!string.IsNullOrEmpty(host))
                return new ProxyServer(proxyProtocol, host, port);

            var literalAddress = match.Groups["address"].Value;
            IPAddress address;
            if (IPAddress.TryParse(literalAddress, out address))
                return new ProxyServer(proxyProtocol, address, port);

            // no host or address was captured
            return null;
        }
    }
}
