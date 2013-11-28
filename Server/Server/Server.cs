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


#region name
//code
#endregion


namespace Server
{
    public class Server
    {
        //private string _message;
        private bool _isRunning;
        private Response _Response;



        // ##########################################################################################################################################
        // Konstruktor
        public Server()     
        {
            _Response = new Response();
        }


        
        // ##########################################################################################################################################
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


        
        // ##########################################################################################################################################
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



        // ############################################################################################################
        // Create Server-Thread (MainThread) for listening
        public void StartServer()
        {
            Thread thread = new Thread(StartServerThread);
            thread.Start();

            _isRunning = true;
        }



        // ##########################################################################################################################################
        // ServerThread ... listening the whole time
        public void StartServerThread()
        {
            Thread.CurrentThread.Name = "ServerThread";

            TcpListener listener = new TcpListener(IPAddress.Any, 8080);
            listener.Start();

            // Plugin Temperatur: Sensor auslesen
            PluginManager PluginManager = new PluginManager("GetTemperatur.html");
            
            // Plugin Navi - Straßenkarte einlesen
            PluginManager = new PluginManager("Navi.html");


            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            Console.WriteLine("Server is running and listen at port 8080 ...");
            Console.WriteLine("Waiting for connection ...");

            separator();

            while (true)
            {
                Socket s = listener.AcceptSocket();

                if (s.Connected)
                {
                    ParameterizedThreadStart ClientThread = new ParameterizedThreadStart(StartClientThread);
                    Thread thread = new Thread(StartClientThread);
                    thread.Start(s);
                }
            }
        }



        // ##########################################################################################################################################
        // Each Client --> new ClientThread
        public void StartClientThread(object socket)
        {
            Thread.CurrentThread.Name = "ClientThread";

            Socket s = (Socket)socket;

            NetworkStream stream = new NetworkStream(s);
            StreamReader sr = new StreamReader(stream);
            StreamWriter sw = new StreamWriter(stream);


            // New Request
            Request RequestPlugin = new Request(sr);

            
            // Wird nur beim ERSTEN Aufruf eine Form ausgegeben
            if (RequestPlugin.Name == "")
            {
                // First Form
                FirstForm FirstForm = new FirstForm();
                FirstForm.CreateFirstForm(sw);
            }
            
            

            // Verhindert doppelte Ausgabe
            if (RequestPlugin.Name != "favicon.ico" && RequestPlugin.Name != null)
            {
                // start PluginManager
                PluginManager PluginManager = new PluginManager(RequestPlugin.Name, RequestPlugin.Parameter, sw);
            }

            
    
            
            //s.Close();
            //stream.Close();
            //sr.Close();
        }
         
    }
}
