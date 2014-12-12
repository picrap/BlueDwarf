using System;
using System.Net;
using System.Net.Sockets;
using BlueDwarf.Net.Proxy.Client;
using Microsoft.Practices.Unity;
using Org.Mentalis.Proxy;

namespace BlueDwarf.Net.Proxy.Server
{
    ///<summary>Listens on a specific port on the proxy server and forwards all incoming HTTP traffic to the appropriate server.</summary>
    public class HttpProxyListener : Listener
    {
        [Dependency]
        public IProxyClient ProxyClient { get; set; }

        public ProxyRoute ProxyRoute { get; set; }

        ///<summary>Initializes a new instance of the HttpListener class.</summary>
        ///<param name="Port">The port to listen on.</param>
        ///<remarks>The HttpListener will start listening on all installed network cards.</remarks>
        public HttpProxyListener(int Port) : this(IPAddress.Any, Port) { }
        ///<summary>Initializes a new instance of the HttpListener class.</summary>
        ///<param name="Port">The port to listen on.</param>
        ///<param name="Address">The address to listen on. You can specify IPAddress.Any to listen on all installed network cards.</param>
        ///<remarks>For the security of your server, try to avoid to listen on every network card (IPAddress.Any). Listening on a local IP address is usually sufficient and much more secure.</remarks>
        public HttpProxyListener(IPAddress Address, int Port) : base(Port, Address) { }
        ///<summary>Called when there's an incoming client connection waiting to be accepted.</summary>
        ///<param name="ar">The result of the asynchronous operation.</param>
        public override void OnAccept(IAsyncResult ar)
        {
            try
            {
                Socket NewSocket = ListenSocket.EndAccept(ar);
                if (NewSocket != null)
                {
                    var NewClient = new HttpProxyClient(NewSocket, new DestroyDelegate(this.RemoveClient)) { Listener = this };
                    AddClient(NewClient);
                    NewClient.StartHandshake();
                }
            }
            catch { }
            try
            {
                //Restart Listening
                ListenSocket.BeginAccept(new AsyncCallback(this.OnAccept), ListenSocket);
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
