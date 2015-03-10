// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Net.Proxy.Client.Diagnostic
{
    /// <summary>
    /// System proxy analyzer interface
    /// </summary>
    public interface ISystemProxyAnalyzer
    {
        /// <summary>
        /// Diagnoses the system proxy.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        ProxyDiagnostic Diagnose(SystemAnalysisParameters parameters = null);
    }
}