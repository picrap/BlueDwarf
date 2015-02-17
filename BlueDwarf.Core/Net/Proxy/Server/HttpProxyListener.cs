// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf
namespace BlueDwarf.Net.Proxy.Server
{
    using System;
    using System.Net;
    using Client;
    using Org.Mentalis.Proxy;

    ///<summary>Listens on a specific port on the proxy server and forwards all incoming HTTP traffic to the appropriate server.</summary>
    public class HttpProxyListener : Listener
    {
        public ProxyRoute ProxyRoute { get; set; }

        ///<summary>Initializes a new instance of the HttpListener class.</summary>
        ///<param name="port">The port to listen on.</param>
        ///<remarks>The HttpListener will start listening on all installed network cards.</remarks>
        public HttpProxyListener(int port) : this(IPAddress.Any, port) { }

        ///<summary>Initializes a new instance of the HttpListener class.</summary>
        ///<param name="port">The port to listen on.</param>
        ///<param name="address">The address to listen on. You can specify IPAddress.Any to listen on all installed network cards.</param>
        ///<remarks>For the security of your server, try to avoid to listen on every network card (IPAddress.Any). Listening on a local IP address is usually sufficient and much more secure.</remarks>
        public HttpProxyListener(IPAddress address, int port)
            : base(port, address)
        {
        }

        ///<summary>Called when there's an incoming client connection waiting to be accepted.</summary>
        ///<param name="ar">The result of the asynchronous operation.</param>
        public override void OnAccept(IAsyncResult ar)
        {
            try
            {
                var newSocket = ListenSocket.EndAccept(ar);
                if (newSocket != null)
                {
                    var newClient = new HttpProxyClient(newSocket, RemoveClient) { Listener = this };
                    AddClient(newClient);
                    newClient.StartHandshake();
                }
            }
            catch { }
            try
            {
                //Restart Listening
                ListenSocket.BeginAccept(OnAccept, ListenSocket);
            }
            catch
            {
                Dispose();
            }
        }
        ///<summary>Returns a string representation of this object.</summary>
        ///<returns>A string with information about this object.</returns>
        public override string ToString()
        {
            return "HTTP service on " + Address.ToString() + ":" + Port.ToString();
        }
        ///<summary>Returns a string that holds all the construction information for this object.</summary>
        ///<value>A string that holds all the construction information for this object.</value>
        public override string ConstructString
        {
            get
            {
                return "host:" + Address.ToString() + ";int:" + Port.ToString();
            }
        }
    }
}
