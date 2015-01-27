// This is the blue dwarf
// more information at https://code.google.com/p/blue-dwarf/

namespace BlueDwarf.Net
{
    using System;

    public static class DownloaderExtensions
    {
        public static string Download(this IDownloader downloader, Uri uri, bool keepTextOnly, params Uri[] proxyServers)
        {
            if (keepTextOnly)
                return downloader.DownloadText(uri, proxyServers);
            return downloader.DownloadRaw(uri, proxyServers);
        }
    }
}
