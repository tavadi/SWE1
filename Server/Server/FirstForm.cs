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

        public void CreateFirstForm(StreamWriter sw)
        {
            string msg =
               @"
                <html>
                    <head> 
                        <title>SensorCloud</title> 
                    </head> 
                    <body>
                        <button><a href=""GetTemperatur.html"">Plugin Temperatur</a></button>
                        <br />
                        <button><a href=""Static.html"">Plugin Static</a></button>
                        <br />
                        <button><a href=""Navi.html"">Plugin Navi</a></button>
                    </body> 
                </html>";

            int size = msg.Length;

            Response Response = new Response();
            Response.ContentType = "text/html";
            Response.sendMessage(sw, msg);
        }
    }
}
