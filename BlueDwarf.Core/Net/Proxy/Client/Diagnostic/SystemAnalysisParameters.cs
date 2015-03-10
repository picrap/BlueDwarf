// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Net.Proxy.Client.Diagnostic
{
    using System;

    public class SystemAnalysisParameters
    {
        public Uri SafeHttpTarget { get; set; }
        public Uri SafeHttpsTarget { get; set; }
        public Uri SensitiveHttpTarget { get; set; }
        public Uri SensitiveHttpsTarget { get; set; }

        public SystemAnalysisParameters()
        {
            SafeHttpTarget = new Uri("http://google.fr");
            SafeHttpsTarget = new Uri("https://google.fr");
            SensitiveHttpTarget = new Uri("http://isohunt.to/");
            SensitiveHttpsTarget = new Uri("https://isohunt.to/");
        }
    }
}