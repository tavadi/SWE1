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
        //private string _message;
        private bool _isRunning;

        // other classes
        private Response _response;
        private Request _request;
        private PluginManager _pluginManager;
        private FirstForm _firstForm;

        // ##########################################################################################################################################
        // Konstruktor
        public Server()
        {
            _response = new Response();
        }

        // ##########################################################################################################################################
        public bool IsRunning
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
        public void Separator()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();

            for (int i = 0; i < 80; ++i)
            {
                Console.Write("-");
            }

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
        }



        // ############################################################################################################
        // Create Server-Thread
        public void StartServer()
        {
            Thread thread = new Thread(StartServerThread);
            thread.Start();

            _isRunning = true;
        }



        // ##########################################################################################################################################
        // ServerThread
        public void StartServerThread()
        {
            Thread.CurrentThread.Name = "ServerThread";

            TcpListener listener = new TcpListener(IPAddress.Any, 8080);
            listener.Start();

            // Plugin Temperatur: Sensor auslesen
            _pluginManager = new PluginManager("Temperatur.html", true);
            
            // Plugin Navi - Straßenkarte einlesen
            _pluginManager = new PluginManager("Navi.html", true);


            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            Console.WriteLine("Server is running and listen at port 8080 ...");
            Console.WriteLine("Waiting for connection ...");

            Separator();

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
            _request = new Request(sr);

            
            // Wird nur beim ERSTEN Aufruf eine Form ausgegeben
            if (_request.Name == "")
            {
                // First Form
                _firstForm = new FirstForm();
                _firstForm.CreateFirstForm(sw);
            }
            
            

            // Verhindert doppelte Ausgabe
            if (_request.Name != "favicon.ico" && _request.Name != null)
            {
                // start PluginManager
                _pluginManager = new PluginManager(_request.Name, _request.Parameter, sw);
            }

            
    
            
            //s.Close();
            //stream.Close();
            //sr.Close();
        }
    }
}
