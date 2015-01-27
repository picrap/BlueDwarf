// This is the blue dwarf
// more information at https://code.google.com/p/blue-dwarf/
namespace BlueDwarf.Net
{
    using System;

    /// <summary>
    /// Interface to file download
    /// </summary>
    public interface IDownloader
    {
        /// <summary>
        /// Downloads the page as text (removes all tags).
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="proxyServers"></param>
        /// <returns></returns>
        string DownloadText(Uri uri, params Uri[] proxyServers);

        /// <summary>
        /// Downloads the page as raw.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="proxyServers">The proxy servers.</param>
        /// <returns></returns>
        string DownloadRaw(Uri uri, params Uri[] proxyServers);
    }
}