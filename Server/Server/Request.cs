using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Web;

namespace Server
{
    class Request
    {
        private IList<string> _Url;
        private IList<string> _Parameter;
        private string _PluginName;
        private List<string> _Header;
        

        public Request(StreamReader sr)
        {
            _Header = new List<string>();



            // Read the Message from Client
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                //Console.WriteLine(line);


                // Split 
                _Url = line.Split(' ');
                

                if (line.StartsWith("GET") && (_Url[1] != "/favicon.ico"))
                {
                    _Header = new List<string>();
                    _Header.Add(_Url[0]);
                    _Header.Add(_Url[1]);
                }

                else if (line.StartsWith("POST"))
                {
                    _Header.Add(_Url[0]);
                    _Header.Add(_Url[1]);
                }

                else if (line.StartsWith("Content-Length"))
                {
                    _Header.Add(_Url[1]);
                }

                if (string.IsNullOrEmpty(line)) break;
            }



            if (_Header.Count > 0)
            {
                if (_Header[0] == "GET")
                {
                    Url URL = new Url(_Header[1]);
                    URL.SplitURLFirst("GET");
                    URL.SplitURLSecond();

                    _PluginName = URL.Name;
                    _Parameter = URL.URL;

                    //Console.WriteLine("GET");
                }

                else if (_Header[0] == "POST")
                {
                    Url URL = new Url(_Header[1], Convert.ToInt32(_Header[2]), sr);
                    URL.SplitURLFirst("POST");
                    URL.SplitURLSecond();

                    _PluginName = URL.Name;
                    _Parameter = URL.URL;

                    //Console.WriteLine("POST");                
                }
            }

        }



        public string Name
        {
            get
            {
                return _PluginName;
            }

            set
            {
            }
        }


        // Return Parameter
        public IList<string> URL
        {
            get
            {
                return _Parameter;
            }
        }


    }
}
