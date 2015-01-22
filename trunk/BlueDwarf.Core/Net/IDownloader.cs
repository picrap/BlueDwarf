// This is the blue dwarf
// more information at https://code.google.com/p/blue-dwarf/
namespace BlueDwarf.Net
{
    using System;
    using Proxy.Client;

    /// <summary>
    /// Interface to file download
    /// </summary>
    public interface IDownloader
    {
        /// <summary>
        /// Downloads the page as text (removes all tags).
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="proxyRoute">The proxy route.</param>
        /// <returns></returns>
        string DownloadText(Uri uri, ProxyRoute proxyRoute);
    }
}