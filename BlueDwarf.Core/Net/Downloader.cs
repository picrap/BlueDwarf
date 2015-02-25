// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Net
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using Annotations;
    using Http;
    using Microsoft.Practices.Unity;
    using Proxy.Client;
    using Proxy.Server;
    using Utility;

    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    public class Downloader : IDownloader
    {
        [Dependency]
        public IProxyServerFactory ProxyServerFactory { get; set; }

        [Dependency]
        public IProxyClient ProxyClient { get; set; }

        /// <summary>
        /// Downloads the page as text (extracts significant text from page) using the specified route.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="proxyServers"></param>
        /// <returns></returns>
        public string DownloadText(Uri uri, params Uri[] proxyServers)
        {
            var proxyRoute = ProxyClient.SafeCreateRoute(uri.Host, uri.Port, proxyServers);
            if (proxyRoute == null)
                return null;
            using (var proxyServer = ProxyServerFactory.CreateSocksProxyServer())
            {
                proxyServer.Port = 0; // auto-select
                proxyServer.ProxyRoute = proxyRoute;
                var textFilePath = Path.GetTempFileName();
                var path = Assembly.GetEntryAssembly().Location;
                var arguments = string.Format("--download={0} --save-text={1} --proxy=socks://localhost:{2}", uri, textFilePath, proxyServer.Port);
                var process = Process.Start(path, arguments);
                process.WaitForExit();
                var text = File.ReadAllText(textFilePath);
                File.Delete(textFilePath);
                return text;
            }
        }

        /// <summary>
        /// Downloads the page as raw.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="proxyServers">The proxy servers.</param>
        /// <returns></returns>
        public string DownloadRaw(Uri uri, params Uri[] proxyServers)
        {
            var proxyRoute = ProxyClient.SafeCreateRoute(uri.Host, uri.Port, proxyServers);
            if (proxyRoute == null)
                return null;

            for (; ; )
            {
                using (var requestStream = proxyRoute.Connect(uri.Host, uri.Port, true))
                {
                    new HttpRequest("GET", uri.AbsolutePath).AddHeader("Host", uri.GetHostAndPort()).AddHeader("Connection", "Close").Write(requestStream);

                    var response = HttpResponse.FromStream(requestStream);
                    var rawContent = response.ReadContentString(requestStream);

                    if (response.StatusCode == 301 || response.StatusCode == 302)
                    {
                        var literalNewLocation = response.Headers["Location"];
                        var newLocation = new Uri(literalNewLocation);
                        if (newLocation.IsAbsoluteUri)
                            uri = newLocation;
                        else
                            uri = new Uri(uri, newLocation);
                        continue;
                    }

                    return rawContent;
                }
            }
        }
    }
}
