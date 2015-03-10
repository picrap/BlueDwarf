// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf

namespace BlueDwarf.Net.Proxy.Client.Diagnostic
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Net.Sockets;
    using Http;
    using Microsoft.Practices.Unity;
    using Name;

    /// <summary>
    /// Proxy analyzer implementation
    /// </summary>
    internal class ProxyAnalyzer : IProxyAnalyzer
    {
        /// <summary>
        /// Gets or sets the name resolver.
        /// </summary>
        /// <value>
        /// The name resolver.
        /// </value>
        [Dependency]
        public INameResolver NameResolver { get; set; }

        /// <summary>
        /// Measures the performance.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <param name="testTarget">The test target.</param>
        /// <param name="tests">The tests.</param>
        /// <returns>A performance value or null if performance could not be measured</returns>
        public ProxyPerformance MeasurePerformance(Route route, Uri testTarget, int tests = 3)
        {
            try
            {
                var ping = TimeSpan.Zero;
                var downloadTime = TimeSpan.Zero;
                var downloadSize = 0;
                var timer = new Stopwatch();
                timer.Start();
                for (int test = 0; test < tests; test++)
                {
                    var t0 = timer.Elapsed;
                    using (var stream = route.Connect(testTarget, NameResolver))
                    {
                        var t1 = timer.Elapsed;
                        ping += t1 - t0;
                        HttpRequest.CreateGet(testTarget).Write(stream);
                        var t2 = timer.Elapsed;
                        var httpResponse = HttpResponse.FromStream(stream);
                        downloadSize += httpResponse.ReadContent(stream).Length;
                        var t3 = timer.Elapsed;
                        downloadTime += t3 - t2;
                    }
                }
                return new ProxyPerformance(TimeSpan.FromTicks(ping.Ticks / tests), downloadSize / downloadTime.TotalSeconds);
            }
            catch (ProxyRouteException)
            { }
            catch (IOException)
            { }
            catch (SocketException)
            { }
            return null;
        }
    }
}
