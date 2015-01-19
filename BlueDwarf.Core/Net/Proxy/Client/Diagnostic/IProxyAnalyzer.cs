// This is the blue dwarf
// more information at https://code.google.com/p/blue-dwarf/
namespace BlueDwarf.Net.Proxy.Client.Diagnostic
{
    public interface IProxyAnalyzer
    {
        ProxyDiagnostic Diagnose(AnalysisParameters parameters = null);
    }
}