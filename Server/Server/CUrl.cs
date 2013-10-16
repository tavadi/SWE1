using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Server
{
    public class CUrl
    {
        private string _DecodedUrl;

        public string DecodeUrl
        {
            get
            {
                return _DecodedUrl;
            }

            set
            {
                // URL Decode
                _DecodedUrl = HttpUtility.UrlDecode(value);
                //Console.WriteLine(url);
            }
        }
    }
}
