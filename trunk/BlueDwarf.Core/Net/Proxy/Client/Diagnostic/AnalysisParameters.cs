using System;

namespace BlueDwarf.Net.Proxy.Client.Diagnostic
{
    public class AnalysisParameters
    {
        public Uri SafeHttpTarget { get; set; }
        public Uri SafeHttpsTarget { get; set; }
        public Uri SensitiveHttpTarget { get; set; }
        public Uri SensitiveHttpsTarget { get; set; }

        public AnalysisParameters()
        {
            SafeHttpTarget = new Uri("http://google.fr");
            SafeHttpsTarget = new Uri("https://google.fr");
            SensitiveHttpTarget = new Uri("http://isohunt.to/");
            SensitiveHttpsTarget = new Uri("https://isohunt.to/");
        }
    }
}