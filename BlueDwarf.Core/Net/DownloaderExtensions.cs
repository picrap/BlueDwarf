// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Net
{
    using System;
    using Proxy.Client;

    public static class DownloaderExtensions
    {
        /// <summary>
        /// Downloads text from specified URI, either as text or raw (html).
        /// </summary>
        /// <param name="downloader">The downloader.</param>
        /// <param name="uri">The URI.</param>
        /// <param name="keepTextOnly">if set to <c>true</c> [keep text only].</param>
        /// <param name="route">The route.</param>
        /// <returns></returns>
        public static string Download(this IDownloader downloader, Uri uri, bool keepTextOnly, Route route)
        {
            if (keepTextOnly)
                return downloader.DownloadText(uri, route);
            return downloader.DownloadRaw(uri, route);
        }
    }
}
