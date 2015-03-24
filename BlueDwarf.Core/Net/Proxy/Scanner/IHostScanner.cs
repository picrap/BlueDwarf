// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Net.Proxy.Scanner
{
    using System.Collections.Generic;

    /// <summary>
    /// Scans a given text for host+port, given a regex
    /// </summary>
    public interface IHostScanner
    {
        /// <summary>
        /// Scans the specified page text.
        /// </summary>
        /// <param name="pageText">The page text.</param>
        /// <param name="hostPortEx">The host port regular expression, or null to use internal capture.</param>
        /// <returns></returns>
        IEnumerable<ProxyServer> Scan(string pageText, string hostPortEx = null);
    }
}