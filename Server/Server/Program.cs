﻿using System;
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

            Server WebServer = new Server();

            WebServer.StartServer();


            // Wait for Input
            Console.ReadLine();
        }
    }
}
