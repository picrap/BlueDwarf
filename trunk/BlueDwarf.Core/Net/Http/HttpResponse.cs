using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BlueDwarf.IO;

namespace BlueDwarf.Net.Http
{
    /// <summary>
    /// Simple HTTP response
    /// (because using the one from the framework was too difficult too)
    /// </summary>
    public class HttpResponse
    {
        // "HTTP/1.1 200 Connection established"
        private string[] _header;
        private string[] Header
        {
            get
            {
                if (_header == null)
                    _header = Lines[0].Split(new[] { ' ' }, 3, StringSplitOptions.RemoveEmptyEntries);
                return _header;
            }
        }

        public int StatusCode
        {
            get
            {
                if (Lines.Count == 0 || Header.Length <= 1)
                    return 0;
                int code;
                int.TryParse(Header[1], out code);
                return code;
            }
        }

        private IDictionary<string, string> _headers;

        /// <summary>
        /// Gets the headers.
        /// </summary>
        /// <value>
        /// The headers.
        /// </value>
        public IDictionary<string, string> Headers
        {
            get
            {
                if (_headers == null)
                {
                    _headers = (from line in Lines.Skip(1)
                                let kvp = line.Split(new[] { ':' }, 2)
                                let kv = Tuple.Create(kvp[0], kvp[1])
                                select kv).ToDictionary(kv => kv.Item1.Trim(), kv => kv.Item2.Trim(), StringComparer.InvariantCultureIgnoreCase);
                }
                return _headers;
            }
        }

        public IList<string> Lines { get; set; }

        public HttpResponse()
        {
            Lines = new List<string>();
        }

        /// <summary>
        /// Reads the response from stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public HttpResponse Read(Stream stream)
        {
            Lines.Clear();
            _header = null;
            for (; ; )
            {
                var responseLine = stream.ReadLineAscii();
                if (responseLine == "")
                    break;

                Lines.Add(responseLine);
            }

            return this;
        }

        /// <summary>
        /// Returns a new response read from stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public static HttpResponse FromStream(Stream stream)
        {
            return new HttpResponse().Read(stream);
        }

        public byte[] ReadContent(Stream stream)
        {
            int? contentLength = null;
            string literalContentLength;
            if (Headers.TryGetValue("Content-Length", out literalContentLength))
            {
                int fContentLength;
                if (int.TryParse(literalContentLength, out fContentLength))
                    contentLength = fContentLength;
            }

            if (!contentLength.HasValue)
            {
                using (var memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    return memoryStream.ToArray();
                }
            }

            var contentBytes = new byte[contentLength.Value];
            stream.ReadAll(contentBytes, 0, contentBytes.Length);
            return contentBytes;
        }

        public string ReadContentString(Stream stream)
        {
            var content = ReadContent(stream);
            return Encoding.Default.GetString(content);
        }
    }
}
