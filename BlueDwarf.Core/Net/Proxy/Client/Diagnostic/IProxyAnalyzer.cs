namespace BlueDwarf.Net.Proxy.Client.Diagnostic
{
    public interface IProxyAnalyzer
    {
        ProxyDiagnostic Diagnose(AnalysisParameters parameters = null);
    }
}