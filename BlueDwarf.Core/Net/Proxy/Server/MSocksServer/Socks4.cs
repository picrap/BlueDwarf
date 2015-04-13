using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
namespace MSocksServer.Socks4Server
{
    using System.Linq;
    using System.Text;

    public class Socks4
    {
        private class Socks4Thread
        {
            private Func<string, int, Socket> _clientConnect;
            private NetworkStream _clientStream;
            private NetworkStream _serverStream;
            private TcpClient _client;
            private TcpClient _server;
            private Thread _transferer;
            private Socks4.onReceiveData OnReceiveData;
            private Socks4.onSendData OnSendData;
            public Socks4Thread(TcpClient Client, Socks4.onReceiveData onReceiveData, Socks4.onSendData onSendData, Func<string, int, Socket> clientConnect)
            {
                this._server = Client;
                this.OnReceiveData = onReceiveData;
                this.OnSendData = onSendData;
                this._transferer = new Thread(new ThreadStart(this.TransfererThread));
                this._transferer.IsBackground = true;
                this._transferer.Start();
                _clientConnect = clientConnect;
            }
            private bool Authorization()
            {
                byte[] buffer = new byte[300];
                byte[] request;
                while (true)
                {
                    if (this._serverStream.DataAvailable)
                    {
                        int num = this._serverStream.Read(buffer, 0, 300);
                        request = new byte[num];
                        Array.Copy(buffer, request, num);
                        if (!request[0].Equals(4))
                        {
                            return false;
                        }
                        switch (request[1])
                        {
                            case 1:
                                goto IL_6A;
                            case 2:
                                return false;
                        }
                    }
                    Thread.Sleep(3);
                }
            IL_6A:
                byte[] portBytes = new byte[2];
                byte[] rawIPv4 = new byte[4];
                // port
                Array.Copy(request, 2, portBytes, 0, 2);
                Array.Reverse(portBytes);
                int port = (int)BitConverter.ToInt16(portBytes, 0);
                // address
                Array.Copy(request, 4, rawIPv4, 0, 4);
                IPAddress ipAddress;
                if (rawIPv4[0] == 0 && rawIPv4[1] == 0 && rawIPv4[2] == 0 && rawIPv4[3] != 0)
                {
                    var endUserNameIndex = IndexOf(request, 0, 8);
                    var endHostNameIndex = IndexOf(request, 0, endUserNameIndex + 1);
                    var host = Encoding.ASCII.GetString(request, endUserNameIndex + 1, endHostNameIndex - endUserNameIndex - 1);
                    ipAddress = Dns.GetHostAddresses(host).First();
                }
                else
                    ipAddress = new IPAddress(rawIPv4);
                byte[] response = new byte[8];
                response[0] = 0;
                try
                {
                    //this._client = new TcpClient(iPAddress.ToString(), port);
                    _client = new TcpClient { Client = _clientConnect(ipAddress.ToString(), port) };
                    response[1] = 90;
                }
                catch (Exception)
                {
                    response[1] = 91;
                }
                this._serverStream.Write(response, 0, response.Length);
                return response[1].Equals(90);
            }

            private static int IndexOf(byte[] array, byte s, int offset)
            {
                while (offset < array.Length)
                {
                    if (array[offset] == s)
                        return offset;
                    offset++;
                }
                return -1;
            }

            private void TransfererThread()
            {
                bool flag = false;
                this._serverStream = this._server.GetStream();
                if (this.Authorization())
                {
                    this._clientStream = this._client.GetStream();
                    bool flag2 = false;
                    string ip = ((IPEndPoint)this._client.Client.RemoteEndPoint).Address.ToString();
                    int port = ((IPEndPoint)this._client.Client.RemoteEndPoint).Port;
                    while (this._client.Connected && this._server.Connected && !flag)
                    {
                        Thread.Sleep(3);
                        try
                        {
                            if (this._serverStream.DataAvailable)
                            {
                                byte[] array = new byte[10000];
                                byte[] array2 = null;
                                int num = this._serverStream.Read(array, 0, 10000);
                                if (num == 0)
                                {
                                    flag = true;
                                }
                                array2 = new byte[num];
                                Array.Copy(array, array2, num);
                                if (this.OnSendData != null)
                                {
                                    this.OnSendData(ref array2, ref flag2, new Socks4ThreadInfo(ip, port));
                                }
                                if (!flag2)
                                {
                                    this._clientStream.Write(array2, 0, num);
                                }
                                flag2 = false;
                            }
                            if (this._clientStream.DataAvailable)
                            {
                                byte[] array3 = new byte[10000];
                                byte[] array4 = null;
                                int num2 = this._clientStream.Read(array3, 0, 10000);
                                if (num2 == 0)
                                {
                                    flag = true;
                                }
                                array4 = new byte[num2];
                                Array.Copy(array3, array4, num2);
                                if (this.OnReceiveData != null)
                                {
                                    this.OnReceiveData(ref array4, ref flag2, new Socks4ThreadInfo(ip, port));
                                }
                                if (!flag2)
                                {
                                    this._serverStream.Write(array4, 0, num2);
                                }
                                flag2 = false;
                            }
                        }
                        catch (Exception)
                        {
                            flag = true;
                        }
                    }
                }
                if (this._client != null && this._client.Connected)
                {
                    this._client.Close();
                }
                if (this._server != null && this._server.Connected)
                {
                    this._server.Close();
                }
            }
        }
        public delegate void onReceiveData(ref byte[] Data, ref bool blocked, Socks4ThreadInfo info);
        public delegate void onSendData(ref byte[] Data, ref bool blocked, Socks4ThreadInfo info);
        private int _port;
        private readonly Func<string, int, Socket> _clientConnect;
        private bool _isOnline;
        private TcpListener _listener;
        private Thread _accepter;
        public event Socks4.onReceiveData OnReceiveData;
        public event Socks4.onSendData OnSendData;
        public int Port
        {
            get
            {
                return this._port;
            }
        }
        public bool isOnline
        {
            get
            {
                return this._isOnline;
            }
        }
        public Socks4(int port, Func<string, int, Socket> clientConnect)
        {
            this._port = port;
            _clientConnect = clientConnect;
            this.Clean();
        }
        public void Clean()
        {
            this.Stop();
            this._listener = new TcpListener(IPAddress.Any, this._port);
            if (_port == 0)
                _port = ((IPEndPoint)_listener.LocalEndpoint).Port;
        }
        public bool Start()
        {
            if (!this._isOnline)
            {
                try
                {
                    this._listener.Start();
                    this._accepter = new Thread(new ThreadStart(this.AccepterThread));
                    this._accepter.IsBackground = true;
                    this._accepter.Start();
                    this._isOnline = true;
                    bool result = true;
                    return result;
                }
                catch (Exception)
                {
                    bool result = false;
                    return result;
                }
            }
            this.Stop();
            return this.Start();
        }
        public void Stop()
        {
            if (this._isOnline)
            {
                if (this._accepter != null)
                {
                    this._accepter.Abort();
                }
                if (this._listener != null)
                {
                    this._listener.Stop();
                }
                this._isOnline = false;
            }
        }
        private void AccepterThread()
        {
            while (true)
            {
                new Socks4.Socks4Thread(this._listener.AcceptTcpClient(), this.OnReceiveData, this.OnSendData, _clientConnect);
            }
        }
    }
}
