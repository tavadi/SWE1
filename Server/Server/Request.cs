using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Web;

namespace Server
{
    public class Request
    {
        private string[] _parameter;
        private string _pluginName;
        private List<string> _header = new List<string>();

        // other classes
        private Url _url;

        // ##########################################################################################################################################
        public Request(StreamReader sr)
        {
            // Read the Message from Client
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                //Console.WriteLine(line);


                // Split 
                string[] url = line.Split(' ');
                
                // Method == GET
                if ((line.StartsWith("GET") && (url[1] != "/favicon.ico")) || (line.StartsWith("POST")))
                {
                    _header.Add(url[0]);
                    _header.Add(url[1]);
                }
                /*
                // Method == POST
                else if (line.StartsWith("POST"))
                {
                    _header.Add(url[0]);
                    _header.Add(url[1]);
                }
                */
                // Content-Length muss für POST gespeichert werden
                else if (line.StartsWith("Content-Length"))
                {
                    _header.Add(url[1]);
                }

                if (string.IsNullOrEmpty(line)) break;
            }



            if (_header.Count > 0)
            {
                // Method == GET
                if (_header[0] == "GET")
                {
                    // Call Constructor with 1 Parameter
                    _url = new Url(_header[1]);

                    _url.SplitUrlFirst("GET");
                    _url.SplitUrlSecond();

                    _pluginName = _url.Name;
                    _parameter = _url.URL;

                    //Console.WriteLine("GET");
                }

                // Method == POST
                else if (_header[0] == "POST")
                {
                    // Call Constructor with 3 Parameter
                    _url = new Url(_header[1], Convert.ToInt32(_header[2]), sr);

                    _url.SplitUrlFirst("POST");
                    _url.SplitUrlSecond();

                    _pluginName = _url.Name;
                    _parameter = _url.URL;

                    //Console.WriteLine("POST");                
                }
            }
        }



        // ##########################################################################################################################################
        public string Name
        {
            get
            {
                return _pluginName;
            }
        }


        // ##########################################################################################################################################
        // Return Parameter
        public string[] Parameter
        {
            get
            {
                return _parameter;
            }
        }


    }
}
