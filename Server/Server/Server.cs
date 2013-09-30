using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    public class Server
    {

        public void StartServer()
        {

            new Thread(StartServerThread).Start();

            
        }


        // ServerThread ... listening the whole time
        public void StartServerThread()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 8080);
            listener.Start();

            Console.WriteLine("Server is running at port 8080 ...");
            Console.WriteLine("Server is waiting for Connection ...");

            while (true)
            {
                Socket s = listener.AcceptSocket();

                ParameterizedThreadStart ThreadStart = new ParameterizedThreadStart(newClient);

                Thread newClientThread = new Thread(newClient);
                newClientThread.Start(s);
            }
        }



        public void newClient(object SessionClient)
        {

            Socket s = (Socket)SessionClient;
            NetworkStream stream = new NetworkStream(s);
            StreamReader sr = new StreamReader(stream);

            Console.WriteLine("Connection accepted from " + s.RemoteEndPoint);

            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                Console.WriteLine(line);
            }
        }

    }
}
