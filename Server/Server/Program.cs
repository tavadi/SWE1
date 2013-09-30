using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;


namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {

            TcpListener listener = new TcpListener(IPAddress.Any, 8080);
            listener.Start();

            Console.WriteLine("Server is running at port 8080 ...");
            Console.WriteLine("Server is waiting for Connection ...");

            Socket s = listener.AcceptSocket();
            NetworkStream stream = new NetworkStream(s);
            StreamReader sr = new StreamReader(stream);

            Console.WriteLine("Connection accepted from " + s.RemoteEndPoint);

            string line = sr.ReadLine();

            while (line != "exit")
            {
                line = sr.ReadLine();
                Console.WriteLine(line);
            }


            Console.WriteLine("Connection close from " + s.RemoteEndPoint);
            Console.ReadLine();
        }
    }
}
