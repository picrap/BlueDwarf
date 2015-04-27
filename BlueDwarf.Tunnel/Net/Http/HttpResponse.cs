// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Net.Http
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using IO;

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

        /// <summary>
        /// Gets the status code.
        /// </summary>
        /// <value>
        /// The status code.
        /// </value>
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

        private IDictionary<string, IList<string>> _headers;

        /// <summary>
        /// Gets the headers.
        /// </summary>
        /// <value>
        /// The headers.
        /// </value>
        public IDictionary<string, IList<string>> Headers
        {
            get
            {
                if (_headers == null)
                {
                    _headers = (from line in Lines.Skip(1)
                                let kvp = line.Split(new[] { ':' }, 2)
                                where kvp.Length == 2
                                let kv = Tuple.Create(kvp[0].Trim(), kvp[1].Trim())
                                select kv)
                                .GroupBy(kv => kv.Item1, kv => kv.Item2, StringComparer.InvariantCultureIgnoreCase)
                                .ToDictionary(kv => kv.Key, kv => (IList<string>)kv.ToList(), StringComparer.InvariantCultureIgnoreCase);
                }
                return _headers;
            }
        }

        /// <summary>
        /// Gets or sets the raw response lines.
        /// </summary>
        /// <value>
        /// The lines.
        /// </value>
        public IList<string> Lines { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpResponse"/> class.
        /// </summary>
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

        /// <summary>
        /// Reads the content.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public byte[] ReadContent(Stream stream)
        {
            int? contentLength = null;
            IList<string> literalContentLength;
            if (Headers.TryGetValue("Content-Length", out literalContentLength))
            {
                int fContentLength;
                if (int.TryParse(literalContentLength.First(), out fContentLength))
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

        /// <summary>
        /// Reads the content as string.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public string ReadContentString(Stream stream)
        {
            var content = ReadContent(stream);
            return Encoding.Default.GetString(content);
        }
    }
}
