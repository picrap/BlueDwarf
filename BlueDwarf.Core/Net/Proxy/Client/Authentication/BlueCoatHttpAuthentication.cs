
using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using BlueDwarf.Net.Http;

namespace BlueDwarf.Net.Proxy.Client.Authentication
{
    public class BlueCoatHttpAuthentication
    {
        private readonly Regex _redirectEx = new Regex(@"\<meta\s+http\-equiv\=\""refresh\""\s+content=\""\d+\;\s*URL\=(?<url>[a-zA-Z0-9\:\/\.]+)");

        public bool Handle(Stream proxyStream, HttpResponse httpResponse, string responseContent, NetworkCredential networkCredential, ProxyRoute routeUntilHere)
        {
            var match = _redirectEx.Match(responseContent);
            if (!match.Success)
                return false;

            var url = match.Groups["url"].Value;
            var uri = new Uri(url);

            using (var redirectStream = routeUntilHere.GetPrevious().Connect(uri.Host, uri.Port, true))
            {
                new HttpRequest("GET", uri.PathAndQuery).Write(redirectStream);
                var redirectResponse = new HttpResponse().Read(redirectStream);
                var redirectResponseContent = redirectResponse.ReadContentString(redirectStream);
            }

            return true;
        }
    }
}
/*
<html>
        <head>
        <title>Redirector</title>
        </head>
        Your request will now be redirected to your requested site.
        <SCRIPT language=javascript>
             document.write('<meta http-equiv="refresh" content="0; URL=http://splash.policyexample.bluecoat.com/');
             document.write(window.location.hostname);
             document.write(window.location.pathname);
             document.write('">');
        </SCRIPT>
        </html>
         */