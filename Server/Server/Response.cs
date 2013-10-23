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
        private LogFile LogFile;

        private string _message;

        // ############################################################################################################
        public IList<string> Message
        {
            set
            {
                try
                {
                    foreach (string i in value)
                    {
                        _message = _message + i + "<br />";
                    }
                }
                catch (NullReferenceException e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Es wurden keine Werte übergeben");
                    Console.ForegroundColor = ConsoleColor.Green;

                    LogFile = new LogFile(e.ToString());
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
                    <body>"
                        + _message +
                        @"
                    </body> 
                </html>";

            HTTPHeader Header = new HTTPHeader(sw, msg);
            Header.sendMessage();

            // Inhalt löschen
            _message = "";
        }

    }
}
