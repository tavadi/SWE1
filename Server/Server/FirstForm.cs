using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Server
{
    public class FirstForm
    {
        // other classes
        private Response _response = new Response();

        public void CreateFirstForm(StreamWriter sw)
        {
            string msg =
               @"
                    <button><a href='Temperatur.html'>Plugin Temperatur</a></button>
                    <br />
                    <button><a href='Static.html'>Plugin Static</a></button>
                    <br />
                    <button><a href='Navi.html'>Plugin Navi</a></button>
                    <br />
                    <button><a href='RssFeed.html'>Plugin RSS-Feed</a></button>
                ";

            int size = msg.Length;

            _response.ContentType = "text/html";
            _response.SendMessage(sw, msg);
        }
    }
}
