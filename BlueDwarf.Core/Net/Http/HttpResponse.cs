using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BlueDwarf.IO;

namespace BlueDwarf.Net.Http
{
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

        public int StatusCode { get { return int.Parse(Header[1]); } }

        private IDictionary<string, string> _headers;
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

        public HttpResponse Read(Stream stream)
        {
            Lines.Clear();
            _header = null;
            for (; ; )
            {
                var responseLine = stream.ReadLineASCII();
                if (responseLine == "")
                    break;

                Lines.Add(responseLine);
            }

            return this;
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
