using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Server
{
    class HTTPHeader
    {
        private StreamWriter _sw;
        private string _msg;

        public HTTPHeader(StreamWriter sw, string msg)
        {
            _sw = sw;
            _msg = msg;
        }

        public void sendMessage()
        {
            _sw.WriteLine("HTTP/1.1 200 OK");
            _sw.WriteLine("Server: Apache/1.3.29 (Unix) PHP/4.3.4");
            _sw.WriteLine("Content-Length: " + _msg.Length);
            _sw.WriteLine("Content-Language: de");
            _sw.WriteLine("Connection: close");
            _sw.WriteLine("Content-Type: text/html");
            _sw.WriteLine();


            //sw.WriteLine(HttpUtility.UrlEncode(msg));
            _sw.WriteLine(_msg);

            _sw.Flush();
        }
    }
}
