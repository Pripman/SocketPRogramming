 using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Collections;

namespace SocketServer
{
	class MainClass
	{
		static int threadNumber;
		static object locker = new object();
		public static void Main (string[] args)
		{
			IPEndPoint ip = new IPEndPoint (IPAddress.Any, 8888);

			Socket listener = new Socket (
				AddressFamily.InterNetwork, 
				SocketType.Stream, 
				ProtocolType.Tcp);
			listener.Bind (ip);
			Console.WriteLine ("TCP server started");
			listener.Listen(1);

			while(true){
				var socket = listener.Accept();
				Console.WriteLine ("\n\n\n\n- Staring new thread for communication");
				var theThread = new Thread (() => handleSocketConnection(socket)); 
				theThread.Start ();
				Console.WriteLine ("-- Thread started..");
			}
		}

		public static void handleSocketConnection(Socket soc)
		{
			lock (locker) {
				threadNumber += 1;
			}
			Console.WriteLine ("--- Hello from thread number {0}", threadNumber);
			byte[] buffer = new byte[1024];
			soc.Receive (buffer, SocketFlags.None);
			string dataRecieved = Encoding.ASCII.GetString (buffer);
			Console.WriteLine("--- Data recieved!\n\n");
			Console.WriteLine (dataRecieved);

			string response = makeResponse ();
			byte[] responseBytes = Encoding.ASCII.GetBytes (response);
			soc.Send (responseBytes);
			Console.WriteLine("--- Response sent to client");
			soc.Close();
			Console.WriteLine ("--- socket closed");
		}

		public static string makeResponse(){
			string response = " HTTP/1.1 200 OK\nAccept-Ranges: bytes\nConnection: close\nContent-Type: text/html\n\n";
			response += "<p>Hello from server to client!<p>";

			return response;
		}
	}
}
