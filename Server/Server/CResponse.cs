using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Web;

namespace Server
{
    public class Response
    {
        private string _message;

        // ############################################################################################################
        public IList<string> Message
        {
            set
            {
                foreach (string i in value)
                {
                    _message = _message + i + "<br />";
                }
            }
        }


        // ############################################################################################################
        public void sendMessage(StreamWriter sw)
        {

            string msg =
                @"
                <html>
                    <head> 
                        <title>SensorCloud</title> 
                    </head> 
                    <body>
                        <h1> "
                            + _message +
                            @"
                        </h1> 
                    </body> 
                </html>";

            sw.WriteLine("HTTP/1.1 200 OK");
            sw.WriteLine("Server: Apache/1.3.29 (Unix) PHP/4.3.4");
            sw.WriteLine("Content-Length: " + msg.Length);
            sw.WriteLine("Content-Language: de");
            sw.WriteLine("Connection: close");
            sw.WriteLine("Content-Type: text/html");
            sw.WriteLine();


            //sw.WriteLine(HttpUtility.UrlEncode(msg));
            sw.WriteLine(msg);

            sw.Flush();

            // Inhalt löschen
            _message = "";
        }

    }
}
