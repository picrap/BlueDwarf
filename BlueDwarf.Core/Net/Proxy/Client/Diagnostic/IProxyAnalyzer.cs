// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Net.Proxy.Client.Diagnostic
{
    public interface IProxyAnalyzer
    {
        ProxyDiagnostic Diagnose(AnalysisParameters parameters = null);
    }
}