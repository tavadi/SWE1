﻿using System;
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

        private string _message;
        private bool _isRunning;


        // Konstruktor
        public Server()     
        { 
        }

        public bool isRunning
        {
            get
            {
                return _isRunning;
            }
            set
            {
                this._isRunning = value;
            }
        }

        public void separator()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();

            for (int i = 0; i < 80; i++)
            {
                Console.Write("-");
            }

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
        }

        public string Message
        {
            get
            {
                return _message;
            }

            set
            {
                _message =
                @"
                <html>
                    <head> 
                        <title>SensorCloud</title> 
                    </head> 
                    <body>
                        <h1> "
                            + value + @" 
                        </h1> 
                    </body> 
                </html>";
            }
        }

        public void sendMessage(StreamWriter sw)
        {
            sw.WriteLine("HTTP/1.1 200 OK");
            sw.WriteLine("Server: Apache/1.3.29 (Unix) PHP/4.3.4");
            sw.WriteLine("Content-Length: " + _message.Length);
            sw.WriteLine("Content-Language: de");
            sw.WriteLine("Connection: close");
            sw.WriteLine("Content-Type: text/html");
            sw.WriteLine();
            sw.WriteLine(_message);

            sw.Flush();
        }







       
        // Create Server-Thread (MainThread) for listening
        public void StartServer()
        {
            Thread thread = new Thread(StartServerThread);
            thread.Start();

            _isRunning = true;
        }

        
        // ServerThread ... listening the whole time
        public void StartServerThread()
        {
            Thread.CurrentThread.Name = "ServerThread";

            TcpListener listener = new TcpListener(IPAddress.Any, 8080);
            listener.Start();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            Console.WriteLine("Server is running and listen at port 8080 ...");
            Console.WriteLine("Waiting for connection ...");

            separator();

            while (true)
            {
                Socket s = listener.AcceptSocket();

                ParameterizedThreadStart threadStart = new ParameterizedThreadStart(StartClientThread);
                Thread thread = new Thread(threadStart);
                thread.Start(s);
            }
              
        }


        // Each Client --> new ClientThread
        public void StartClientThread(object socket)
        {
            Thread.CurrentThread.Name = "ClientThread";

            Socket s = (Socket)socket;

            NetworkStream stream = new NetworkStream(s);
            StreamReader sr = new StreamReader(stream);
            StreamWriter sw = new StreamWriter(stream);

            /*
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Connection accepted from " + s.RemoteEndPoint);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            */

            // Read the Message from Client
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                Console.WriteLine(line);
                if (string.IsNullOrEmpty(line)) break;
            }


            // Build a Message for the Client
            string message = "Done!";

            this.Message = message;      // Create Message from Server to Client
            sendMessage(sw);           // Send the Message to the Client
            
            
            /*
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Conneciton close from " + s.RemoteEndPoint);
            Console.ForegroundColor = ConsoleColor.White;
            
            separator();
            */

            s.Close();
            stream.Close();
            sr.Close();
            sw.Close();            
        }
         
    }
}
