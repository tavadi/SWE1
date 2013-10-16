using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Server
{
    public class CFirstForm
    {

        public void CreateFirstForm(StreamWriter sw)
        {
            string msg =
               @"
                <html>
                    <head> 
                        <title>SensorCloud</title> 
                    </head> 
                    <body>
                        <form method=POST action=""CGetTemperatur.html"">
                            <input type=""text"" name=""input"" />
                            <input type=""submit"" value=""Submit"" />
                        </form>
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
        }
    }
}
