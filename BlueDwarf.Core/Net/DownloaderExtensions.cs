// This is the blue dwarf
// more information at https://code.google.com/p/blue-dwarf/

namespace BlueDwarf.Net
{
    using System;

    public static class DownloaderExtensions
    {
        /// <summary>
        /// Downloads text from specified URI, either as text or raw (html).
        /// </summary>
        /// <param name="downloader">The downloader.</param>
        /// <param name="uri">The URI.</param>
        /// <param name="keepTextOnly">if set to <c>true</c> [keep text only].</param>
        /// <param name="proxyServers">The proxy servers.</param>
        /// <returns></returns>
        public static string Download(this IDownloader downloader, Uri uri, bool keepTextOnly, params Uri[] proxyServers)
        {
            if (keepTextOnly)
                return downloader.DownloadText(uri, proxyServers);
            return downloader.DownloadRaw(uri, proxyServers);
        }
    }
}
