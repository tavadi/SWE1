using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;


namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server();

            server.StartServer();

            while (server.IsRunning)
            {
                string input = Console.ReadLine();

                if (input.ToLower() == "exit")
                {
                    //WebServer.isRunning = false;
                    Environment.Exit(1);
                }
            }
            // Wait for Input
            Console.ReadLine();
        }
    }
}
