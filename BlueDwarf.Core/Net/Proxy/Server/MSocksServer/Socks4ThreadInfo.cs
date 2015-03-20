using System;
namespace MSocksServer.Socks4Server
{
	public class Socks4ThreadInfo
	{
		private string _ip;
		private int _port;
		public string Ip
		{
			get
			{
				return this._ip;
			}
		}
		public int Port
		{
			get
			{
				return this._port;
			}
		}
		public Socks4ThreadInfo(string ip, int port)
		{
			this._ip = ip;
			this._port = port;
		}
	}
}
