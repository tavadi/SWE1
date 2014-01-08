using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.IO;

namespace Server
{
    public class Url
    {
        private StreamReader _sr;

        private string _url;
        private string _pluginName;
        private string _splitFirst;
        private string _decodedUrl;

        private string[] _urlSplit;
        private int _contentLength;


        // ##########################################################################################################################################
        public Url(string requestUrl)
        {
            _url = requestUrl;
        }


        // ##########################################################################################################################################
        public Url(string requestUrl, int contentLength, StreamReader sr)
        {
            _url = requestUrl;
            _contentLength = contentLength;
            _sr = sr;
        }




        // ##########################################################################################################################################
        public void SplitUrlFirst(string method)
        {
            _urlSplit = _url.Split( new char [] {'?', '/'}, 3);

            // Pluginname steht immer an der 1ten Stelle
            _pluginName = _urlSplit[1];

            if (method == "GET")
            {
                SplitGet();
            }
            else if (method == "POST")
            {
                SplitPost();
            }
        }


        // ##########################################################################################################################################
        public void SplitUrlSecond()
        {
            _urlSplit = _splitFirst.Split('&', '=', '/');
        }



        // ##########################################################################################################################################
        private void SplitGet()
        {
            // Je nachdem wieviele Werte übergeben wurden, müssen die Parameter angepasst werden.
            // [1] = PluginName
            if (_urlSplit.Length <= 2)
            {
                _decodedUrl = HttpUtility.UrlDecode(_urlSplit[1]);
                _decodedUrl = _decodedUrl.Replace(" ", "");
                _urlSplit[1] = _decodedUrl;
            }

            // [1] = Pluginname
            // [2] = Parameter
            else
            {
                _decodedUrl = HttpUtility.UrlDecode(_urlSplit[2]);
                _decodedUrl = _decodedUrl.Replace(" ", "");
                _urlSplit[2] = _decodedUrl;
            }


            foreach (string s in _urlSplit)
            {
                _splitFirst = s;
            }
        }


        // ##########################################################################################################################################
        private void SplitPost()
        {                
            // Werte aus dem Body filtern
            var buffer = new char[_contentLength];

            _sr.Read(buffer, 0, _contentLength);
            _splitFirst = Encoding.UTF8.GetString(buffer.Select(c => (byte)c).ToArray());
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
        public string[] URL
        {
            get
            {
                return _urlSplit;
            }
        }
    }
}
