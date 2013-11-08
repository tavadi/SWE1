﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Web;

namespace Server
{
    class HTTPHeader
    {
        private StreamWriter _sw;
        private string _msg;
        private string _ContentType;
        private int _size;




        // ##########################################################################################################################################
        public HTTPHeader(StreamWriter sw, string msg, string ContentType, int size)
        {
            _sw = sw;
            _msg = msg;
            _ContentType = ContentType;
            _size = size;
        }




        // ##########################################################################################################################################
        public void sendMessage()
        {
            try
            {
                _sw.WriteLine("HTTP/1.1 200 OK");
                _sw.WriteLine("Server: Apache/1.3.29 (Unix) PHP/4.3.4");
                _sw.WriteLine("Content-Length: " + _size);
                _sw.WriteLine("Content-Language: de");
                _sw.WriteLine("Connection: close");
                _sw.WriteLine("Content-Type: " + _ContentType);
                _sw.WriteLine();


                //_sw.WriteLine(HttpUtility.UrlEncode(_msg));
                _sw.WriteLine(_msg);

                _sw.Flush();
            }
            catch (IOException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Fehler im Header" + e);
                Console.ForegroundColor = ConsoleColor.Green;
            }
        }
    }
}
