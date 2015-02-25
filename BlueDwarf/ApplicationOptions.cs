// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf
{
    using CommandLine;

    public class ApplicationOptions
    {
        [Option('p', "proxy-port", Required = false, HelpText = "Sockets proxy server port.")]
        public int ProxyPort { get; set; }

        [Option('m', "minimized", Required = false, HelpText = "Starts minimized.")]
        public bool Minimized { get; set; }

        [Option('d', "download", Required = false, HelpText = "Downloads the URI.")]
        public string DownloadUri { get; set; }

        [Option('t', "save-text", Required = false, HelpText = "Saves the file as text.")]
        public string SaveTextPath { get; set; }

        [Option("proxy", Required = false, HelpText = "Sets proxy (for downloading URIs).")]
        public string Proxy { get; set; }
    }
}
