using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Server;

namespace Server
{
    public class Static : IPlugins
    {

        private string _PluginName = "Static.html";
        private bool _isPlugin = false;

        private IList<string> _Parameter;
        private string _Response;

        private string _ContentType;

        private Response Resp = new Response();

        private StreamWriter _sw;



        // ##########################################################################################################################################
        public StreamWriter Writer
        {
            set
            {
                _sw = value;
            }
        }


        // ##########################################################################################################################################
        public string PluginName
        {
            get
            {
                Console.WriteLine("ICH BIN DAS PLUGIN: " + _PluginName);
                return _PluginName;
            }
        }


        // ##########################################################################################################################################
        public bool isPlugin
        {
            set
            {
                _isPlugin = value;
            }

            get
            {
                return _isPlugin;
            }
        }



        // ##########################################################################################################################################
        public IList<string> doSomething
        {
            set
            {
                _Parameter = value;

                if (_Parameter[0] == _PluginName)
                {
                    showFiles();
                }
                else
                {
                    openFiles();
                }
            }
        }



        // ##########################################################################################################################################
        public string ContentType
        {
            get
            {
                return _ContentType;
            }
        }



        // ##########################################################################################################################################
        private void showFiles()
        {
            string path = Environment.CurrentDirectory + "\\Files\\";

            foreach (string file in System.IO.Directory.GetFiles(path, "*"))
            {
                IList<string> Split;

                Split = file.Split('\\');


                _Response += @"
                    <button><a href=Static.html?" + Split.Last() + ">" + Split.Last() + @"</a></button>
                    <br />
                ";


            }

            _ContentType = "text/html";
            Resp.sendMessage(_sw, _Response, _ContentType);  
        }


        // ##########################################################################################################################################
        private void openFiles()
        {
            
            string path = Environment.CurrentDirectory + "\\Files\\" + _Parameter[0];
            
            byte[] FileContent;
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                FileContent = new byte[fs.Length];
                fs.Read(FileContent, 0, Convert.ToInt32(fs.Length));
            }


            string[] FileExtension = _Parameter[0].Split('.');

            switch (FileExtension[1])
            {
                case "jpg":
                    _ContentType = "image/jpeg";
                    break;

                case "png":
                    _ContentType = "image/png";
                    break;

                case "gif":
                    _ContentType = "image/gif";
                    break;

                case "txt":
                    _ContentType = "image/plain";
                    break;

                case "xml":
                    _ContentType = "image/xml";
                    break;

                default:
                    _ContentType = "application/octet-stream";
                    break;
            }

            
            Resp.sendMessage(_sw, FileContent, _ContentType);

             
        }
    }
}
