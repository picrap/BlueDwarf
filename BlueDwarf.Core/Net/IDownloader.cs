// This is the blue dwarf
// more information at https://code.google.com/p/blue-dwarf/
namespace BlueDwarf.Net
{
    using System;
    using Proxy.Client;

    public interface IDownloader
    {
        string DownloadText(Uri uri, ProxyRoute proxyRoute);
    }
}