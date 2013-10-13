using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Server
{
    public class Response
    {
        private string _message;

        // ############################################################################################################
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


        // ############################################################################################################
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

    }
}
