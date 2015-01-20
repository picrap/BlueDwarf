// This is the blue dwarf
// more information at https://code.google.com/p/blue-dwarf/

namespace BlueDwarf.Net
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using Annotations;
    using Microsoft.Practices.Unity;
    using Proxy.Client;
    using Proxy.Server;

    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
    public class Downloader : IDownloader
    {
        [Dependency]
        public IProxyServerFactory ProxyServerFactory { get; set; }

        /// <summary>
        /// Downloads the page as text (extracts significant text from page) using the specified route.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="proxyRoute">The proxy route.</param>
        /// <returns></returns>
        public string DownloadText(Uri uri, ProxyRoute proxyRoute)
        {
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
    }
}
