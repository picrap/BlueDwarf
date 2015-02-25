// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Net.Http
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    /// <summary>
    /// Simple HTTP request
    /// (because using the one from the framework was too difficult)
    /// </summary>
    public class HttpRequest
    {
        public string Verb { get; set; }
        public string TargetPath { get; set; }
        public int? TargetPort { get; set; }

        public string Target
        {
            get
            {
                if (!TargetPort.HasValue)
                    return TargetPath;
                return string.Format("{0}:{1}", TargetPath, TargetPort.Value);
            }
        }

        public IList<KeyValuePair<string, string>> Headers { get; set; }

        private const string NewLine = "\r\n";

        public HttpRequest()
        {
            Headers = new List<KeyValuePair<string, string>>();
            Verb = "GET";
        }

        public HttpRequest(string verb, string targetPath, int? targetPort = null)
        {
            Headers = new List<KeyValuePair<string, string>>();
            Verb = verb;
            TargetPath = targetPath;
            TargetPort = targetPort;
        }

        public HttpRequest AddHeader(string key, string value)
        {
            Headers.Add(new KeyValuePair<string, string>(key, value));
            return this;
        }

        public void Write(Stream stream, string postContent = null)
        {
            var requestBuilder = new StringBuilder();
            requestBuilder.AppendFormat(@"{0} {1} HTTP/1.1", Verb, Target).Append(NewLine);
            if (postContent != null)
                AddHeader("Content-Length", postContent.Length.ToString());
            foreach (var header in Headers)
                requestBuilder.AppendFormat(@"{0}: {1}", header.Key, header.Value).Append(NewLine);
            requestBuilder.Append(NewLine);

            var streamWriter = new StreamWriter(stream, Encoding.ASCII);
            streamWriter.Write(requestBuilder.ToString());
            if (postContent != null)
                streamWriter.Write(postContent);
            streamWriter.Flush();
        }
    }
}