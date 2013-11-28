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
        private string[] _Parameter;
        private string _PluginName;
        private List<string> _Header;




        // ##########################################################################################################################################
        public Request(StreamReader sr)
        {
            // HTTP-Header Informationen
            _Header = new List<string>();


            // Read the Message from Client
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                //Console.WriteLine(line);


                // Split 
                string[] Url = line.Split(' ');
                
                // Method == GET
                if (line.StartsWith("GET") && (Url[1] != "/favicon.ico"))
                {
                    //string[] Header;
                    
                    _Header = new List<string>();
                    _Header.Add(Url[0]);
                    _Header.Add(Url[1]);
                    
                }

                // Method == POST
                else if (line.StartsWith("POST"))
                {
                    _Header.Add(Url[0]);
                    _Header.Add(Url[1]);
                }

                // Content-Length muss für POST gespeichert werden
                else if (line.StartsWith("Content-Length"))
                {
                    _Header.Add(Url[1]);
                }

                if (string.IsNullOrEmpty(line)) break;
            }



            if (_Header.Count > 0)
            {
                // Method == GET
                if (_Header[0] == "GET")
                {
                    // Call Contructor with 1 Parameter
                    Url URL = new Url(_Header[1]);

                    URL.SplitURLFirst("GET");
                    URL.SplitURLSecond();

                    _PluginName = URL.Name;
                    _Parameter = URL.URL;

                    //Console.WriteLine("GET");
                }

                // Method == POST
                else if (_Header[0] == "POST")
                {
                    // Call Constructor with 3 Parameter
                    Url URL = new Url(_Header[1], Convert.ToInt32(_Header[2]), sr);

                    URL.SplitURLFirst("POST");
                    URL.SplitURLSecond();

                    _PluginName = URL.Name;
                    _Parameter = URL.URL;

                    //Console.WriteLine("POST");                
                }
            }
        }





        // ##########################################################################################################################################
        public string Name
        {
            get
            {
                return _PluginName;
            }
        }





        // ##########################################################################################################################################
        // Return Parameter
        public string[] Parameter
        {
            get
            {
                return _Parameter;
            }
        }


    }
}
