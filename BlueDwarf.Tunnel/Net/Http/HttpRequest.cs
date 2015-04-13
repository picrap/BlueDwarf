// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Net.Http
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using Utility;

    /// <summary>
    /// Simple HTTP request
    /// (because using the one from the framework was too difficult)
    /// </summary>
    public class HttpRequest
    {
        /// <summary>
        /// Gets or sets the verb.
        /// </summary>
        /// <value>
        /// The verb.
        /// </value>
        public string Verb { get; set; }
        /// <summary>
        /// Gets or sets the target path.
        /// </summary>
        /// <value>
        /// The target path.
        /// </value>
        public string TargetPath { get; set; }
        /// <summary>
        /// Gets or sets the target port.
        /// </summary>
        /// <value>
        /// The target port.
        /// </value>
        public int? TargetPort { get; set; }

        /// <summary>
        /// Gets the target.
        /// </summary>
        /// <value>
        /// The target.
        /// </value>
        public string Target
        {
            get
            {
                if (!TargetPort.HasValue)
                    return TargetPath;
                return string.Format("{0}:{1}", TargetPath, TargetPort.Value);
            }
        }

        /// <summary>
        /// Gets or sets the headers.
        /// </summary>
        /// <value>
        /// The headers.
        /// </value>
        public IList<KeyValuePair<string, string>> Headers { get; set; }

        private const string NewLine = "\r\n";

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpRequest"/> class.
        /// </summary>
        public HttpRequest()
        {
            Headers = new List<KeyValuePair<string, string>>();
            Verb = "GET";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpRequest"/> class.
        /// </summary>
        /// <param name="verb">The verb.</param>
        /// <param name="targetPath">The target path.</param>
        /// <param name="targetPort">The target port.</param>
        public HttpRequest(string verb, string targetPath, int? targetPort = null)
        {
            Headers = new List<KeyValuePair<string, string>>();
            Verb = verb;
            TargetPath = targetPath;
            TargetPort = targetPort;
        }

        /// <summary>
        /// Creates a GET HttpRequest.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>
        public static HttpRequest CreateGet(Uri uri)
        {
            return new HttpRequest("GET", uri.AbsolutePath).AddHeader("Host", uri.GetHostAndPort()).AddHeader("Connection", "Close");
        }

        /// <summary>
        /// Creates a CONNECT HttpRequest.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="port">The port.</param>
        /// <returns></returns>
        public static HttpRequest CreateConnect(string host, int port)
        {
            return new HttpRequest("CONNECT", host, port).AddHeader("Proxy-Connection", "Keep-Alive");
        }

        /// <summary>
        /// Appends the header.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public HttpRequest AddHeader(string key, string value)
        {
            Headers.Add(new KeyValuePair<string, string>(key, value));
            return this;
        }

        /// <summary>
        /// Writes the request to a given stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="postContent">Content of the post.</param>
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