﻿using System;
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

        private string _Url;
        private string _PluginName;
        private string _SplitFirst;
        private string _DecodedUrl;

        private IList<string> _UrlSplit;
        private int _ContentLength;


        public Url(string RequestUrl)
        {
            _Url = RequestUrl;
        }

        public Url(string RequestUrl, int ContentLength, StreamReader sr)
        {
            _Url = RequestUrl;
            _ContentLength = ContentLength;
            _sr = sr;
        }



        public void SplitURLFirst(string Method)
        {
            _UrlSplit = _Url.Split('?', '/');

            // Pluginname steht immer an der 1ten Stelle
            _PluginName = _UrlSplit[1];
 
            if (Method == "GET")
            {
                if (_UrlSplit.Count <= 2)
                {
                    _DecodedUrl = HttpUtility.UrlDecode(_UrlSplit[1]);
                    _DecodedUrl = _DecodedUrl.Replace(" ", "");
                    _UrlSplit[1] = _DecodedUrl;
                }
                else
                {
                    _DecodedUrl = HttpUtility.UrlDecode(_UrlSplit[2]);
                    _DecodedUrl = _DecodedUrl.Replace(" ", "");
                    _UrlSplit[2] = _DecodedUrl;
                }
                foreach (string s in _UrlSplit)
                {
                    _SplitFirst = s;
                }
            }
            else if (Method == "POST")
            {
                // Werte aus dem Body filtern
                var buffer = new char[_ContentLength];

                _sr.Read(buffer, 0, _ContentLength);
                _SplitFirst = Encoding.UTF8.GetString(buffer.Select(c => (byte)c).ToArray());
            }
        }


        public void SplitURLSecond()
        {
            _UrlSplit = _SplitFirst.Split('&', '=');
        }




        public string Name
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
                return _UrlSplit;
            }
        }
    }
}
