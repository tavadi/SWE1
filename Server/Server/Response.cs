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
        private string _contentType;
        private bool _status = true;
        private string _filename;

        private int _size;

        private StreamWriter _sw;

        // other classes
        private Html html = new Html();

        
        // ##########################################################################################################################################
        public void SendMessage(StreamWriter sw, string response)
        {
            _sw = sw;

            try
            {
                // Wenn XML --> nur die reinen XML-Daten zurückschicken
                if (_contentType == "text/html")
                {
                    _size = html.HtmlHeader.Length + html.HtmlFooter.Length + response.Length;

                    // Response abschicken
                    SendHttpHeader();

                    _sw.WriteLine(html.HtmlHeader);
                    _sw.WriteLine(response);
                    _sw.WriteLine(html.HtmlFooter);
                }

                // Wenn es "text/html" ist, die Nachrichten in die HTML-Seite einbetten
                else if (_contentType == "text/xml")
                {
                    _size = response.Length;

                    SendHttpHeader();
                    _sw.WriteLine(response);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e);

                response = "Error: " + e;

                _size = response.Length;
                SendHttpHeader();
                _sw.WriteLine(response);
            }

            _sw.Flush();

        }



        // ##########################################################################################################################################
        public void SendMessage(StreamWriter sw, byte[] message, string filename)
        {
            _sw = sw;
            _size = message.Length;
            _filename = filename;


            SendHttpHeader();
            sw.Flush();
            sw.BaseStream.Write(message, 0, message.Length);
            sw.Flush();

        }


        // ##########################################################################################################################################
        public string ContentType
        {
            set
            {
                _contentType = value;
            }
        }

        // ##########################################################################################################################################
        public bool Status
        {
            set
            {
                _status = value;
            }
        }



        // ##########################################################################################################################################
        private void SendHttpHeader()
        {
            if (_status == true)
                _sw.WriteLine("HTTP/1.1 200 OK");
            else if (_status == false)
                _sw.WriteLine("HTTP/1.1 404 Not Found");

            _sw.WriteLine("Server: localhost:8080 (Unix) PHP/4.3.4");
            _sw.WriteLine("Content-Type: " + _contentType + "; charset=UTF-8");         // charset = Umlaute im Browser
            _sw.WriteLine("Content-Length: " + _size);
            _sw.WriteLine("Content-Language: de");

            if (_contentType == "application/octet-stream")
                _sw.WriteLine("Content-Disposition: attachment; filename=" + _filename);

            _sw.WriteLine("Connection: close"); 
            _sw.WriteLine();
        }
    }
}
