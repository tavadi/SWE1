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


#region name
//code
#endregion


namespace Server
{
    public class CServer
    {
        //private string _message;
        private bool _isRunning;
        private Response _Response;
        private List<string> _Plugins;
        private string _PluginName;


        // ############################################################################################################ 
        // Konstruktor
        public CServer()     
        {
            _Response = new Response();
        }


        // ############################################################################################################
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


        // ############################################################################################################
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


        // ############################################################################################################
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


        // ############################################################################################################
        // Each Client --> new ClientThread
        public void StartClientThread(object socket)
        {
            Thread.CurrentThread.Name = "ClientThread";

            Socket s = (Socket)socket;

            NetworkStream stream = new NetworkStream(s);
            StreamReader sr = new StreamReader(stream);
            StreamWriter sw = new StreamWriter(stream);


            // Read the Message from Client
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                //Console.WriteLine(line);

                string url = HttpUtility.UrlDecode(line);
                //Console.WriteLine(url);

                var array = url.Split('/', ' ');
                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i] == "GET")
                    {
                        _PluginName = array[i + 2];
                    }
                }

                if (string.IsNullOrEmpty(line)) break;
            }


            // FAVICON-THREAD wird beendet
            if (_PluginName == "favicon.ico")
            {
                return;
            }

            Console.WriteLine(_PluginName);


            // Build a Message for the Client
            string message = "Done!";

            _Response.Message = message;      // Create Message from Server to Client
            _Response.sendMessage(sw);           // Send the Message to the Client
            
            
            s.Close();
            stream.Close();
            sr.Close();
            sw.Close();            
        }
         

        // ############################################################################################################
        // Read txt-File
        public void ReadPlugins()
        {
            string line;
            int counter = 0;

            _Plugins = new List<string>();


            StreamReader file = new StreamReader("../../Plugins.txt");

            while ((line = file.ReadLine()) != null)
            {
                //Console.WriteLine(line);
                _Plugins.Add(line);
                counter++;
            }

            file.Close();
        }
    }
}
