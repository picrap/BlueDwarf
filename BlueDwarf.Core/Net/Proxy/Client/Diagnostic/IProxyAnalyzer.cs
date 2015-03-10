namespace BlueDwarf.Net.Proxy.Client.Diagnostic
{
    using System;

    public interface IProxyAnalyzer
    {
        /// <summary>
        /// Measures the performance.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <param name="testTarget">The test target.</param>
        /// <param name="tests">The tests.</param>
        /// <returns></returns>
        ProxyPerformance MeasurePerformance(Route route, Uri testTarget, int tests = 3);
    }
}