using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Web;


namespace Server
{
    public class Server
    {
        bool _ServerIsRunning;

        // Konstruktor
        public Server()     {    _ServerIsRunning = true;   }


        // SETTER
        public void setServerStatus(bool status)
        {
            _ServerIsRunning = status;
        }

        // GETTER
        public bool getServerStatus()
        {
            return _ServerIsRunning;
        }





        // Create Server-Thread (MainThread) for listening
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
                // If Client connect --> new ClientThread
                Socket s = listener.AcceptSocket();

                ParameterizedThreadStart ThreadStart = new ParameterizedThreadStart(ClientThread);
                Thread newClientThread = new Thread(ClientThread);
                newClientThread.Start(s);
            }
        }



        public void ClientThread(object SessionClient)
        {
            Socket s = (Socket)SessionClient;
            NetworkStream stream = new NetworkStream(s);
            StreamReader sr = new StreamReader(stream);
            StreamWriter sw = new StreamWriter(stream);
            Console.WriteLine("Connection accepted from " + s.RemoteEndPoint);

            
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                Console.WriteLine(line);
            }
            

            
        }
    }
}
