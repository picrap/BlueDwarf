﻿// This is the blue dwarf
// more information at https://code.google.com/p/blue-dwarf/

namespace BlueDwarf.Net.Proxy.Client.Diagnostic
{
    using System;

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