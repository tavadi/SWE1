using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Web;

namespace Server
{
    class CRequest
    {
        private IList<string> _list;
        private List<string> _URLList;
        private string _url;
        private string _PluginName;


        public CRequest(StreamReader sr)
        {
            // Read the Message from Client
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                //Console.WriteLine(line);

                _url = HttpUtility.UrlDecode(line);
                //Console.WriteLine(url);

                _list = _url.Split('/', ' ');


                for (int i = 0; i < _list.Count; i++)
                {
                    if ((_list[i] == "GET") && (_list[2] != "favicon.ico"))
                    {
                        _PluginName = _list[i + 2];

                        createList();
                    }
                }

                if (string.IsNullOrEmpty(line)) break;
            }


        }


        private void createList()
        {
            _URLList = new List<string>();

            int i = 3;
            while (_list[i] != "HTTP")
            {
                _URLList.Add(_list[i]);
                i++;
            }
        }        



        // Teturn PluginName
        public string PluginName
        {
            get
            {
                return _PluginName;
            }
        }


        // Return Parameter
        public IList<string> URL
        {
            get
            {
                return _URLList;
            }
        }
    }
}
