using System;
using System.Net.Sockets;
using System.Net;
using System.Text;


namespace SocketClient
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			IPEndPoint ip = new IPEndPoint (IPAddress.Parse("127.0.0.1"), 8888);

			Socket socket = new Socket (				
				AddressFamily.InterNetwork, 
				SocketType.Stream, 
				ProtocolType.Tcp);
			//socket.Bind (new IPEndPoint (IPAddress.Parse ("localhost"), 8889));
			socket.Connect(ip);
			string request = makeRequest ();
			byte[] requestBytes = Encoding.ASCII.GetBytes (request);
			socket.Send (requestBytes);
			byte[] buffer = new byte[1024];
			socket.Receive (buffer, SocketFlags.None);
			string dataRecieved = Encoding.ASCII.GetString (buffer);
			Console.WriteLine (dataRecieved);
			socket.Close ();
		}
		public static string makeRequest(){
			string request = "Hello to server from client";
			return request;

		}
	}
}
