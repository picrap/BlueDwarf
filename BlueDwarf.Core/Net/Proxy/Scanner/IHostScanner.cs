// This is the blue dwarf
// more information at https://code.google.com/p/blue-dwarf/

namespace BlueDwarf.Net.Proxy.Scanner
{
    using System.Collections.Generic;

    public interface IHostScanner
    {
        /// <summary>
        /// Scans the specified page text.
        /// </summary>
        /// <param name="pageText">The page text.</param>
        /// <param name="hostPortEx">The host port regular expression, or null to use internal capture.</param>
        /// <returns></returns>
        IEnumerable<HostPort> Scan(string pageText, string hostPortEx = null);
    }
}