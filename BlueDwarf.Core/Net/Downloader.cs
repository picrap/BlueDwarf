// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Net
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Annotations;
    using Http;
    using Microsoft.Practices.Unity;
    using Name;
    using Proxy.Client;
    using Proxy.Server;
    using Utility;

    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    public class Downloader : IDownloader
    {
        [Dependency]
        public IProxyServerFactory ProxyServerFactory { get; set; }

        /// <summary>
        /// Gets or sets the name resolver.
        /// </summary>
        /// <value>
        /// The name resolver.
        /// </value>
        [Dependency]
        public INameResolver NameResolver { get; set; }

        /// <summary>
        /// Downloads the page as text (extracts significant text from page) using the specified route.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="route">The route.</param>
        /// <returns></returns>
        public string DownloadText(Uri uri, Route route)
        {
            using (var proxyServer = ProxyServerFactory.CreateSocksProxyServer())
            {
                proxyServer.Port = 0; // auto-select
                proxyServer.Routes = new[] { route };
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
        /// <param name="route">The route.</param>
        /// <returns></returns>
        public string DownloadRaw(Uri uri, Route route)
        {
            for (; ; )
            {
                using (var requestStream = route.Connect(uri, NameResolver))
                {
                    HttpRequest.CreateGet(uri).Write(requestStream);
                    var response = HttpResponse.FromStream(requestStream);
                    var rawContent = response.ReadContentString(requestStream);

                    if (response.StatusCode == 301 || response.StatusCode == 302)
                    {
                        var literalNewLocation = response.Headers["Location"].Single();
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
