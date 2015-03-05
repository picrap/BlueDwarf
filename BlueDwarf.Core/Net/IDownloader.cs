// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
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
        /// <param name="route">The route.</param>
        /// <returns></returns>
        string DownloadText(Uri uri, Route route);

        /// <summary>
        /// Downloads the page as raw.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="route">The route.</param>
        /// <returns></returns>
        string DownloadRaw(Uri uri, Route route);
    }
}