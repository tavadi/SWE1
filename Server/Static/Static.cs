using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Web;
using Server;

namespace Server
{
    public class Static : IPlugins
    {

        private string _PluginName = "Static.html";
        private bool _isPlugin = false;

        private string[] _Parameter;
        private string _Response;

        private string _ContentType;

        private StreamWriter _sw;

        private Response _Resp = new Response();




        // ##########################################################################################################################################
        // Streamwriter
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
        public string[] doSomething
        {
            set
            {
                // Parameter werden übergeben
                _Parameter = value;

                // Erster Aufruf --> Übersichtsseite
                if (_Parameter[0] == _PluginName)
                {
                    showFiles();
                }

                // Weitere Aufrufe --> Fileaufruf
                else
                {
                    openFiles();
                }
            }
        }



        // ##########################################################################################################################################
        private void showFiles()
        {
            // Pfad am Server --> gesamter Inhalt wird ausgegeben
            string path = Environment.CurrentDirectory + "\\Files\\";

            // Für jedes File
            foreach (string file in System.IO.Directory.GetFiles(path, "*"))
            {
                // Pfad wird gesplittet --> nur der Filename inkl. Endung wird benötigt
                IList<string> Split = file.Split('\\');

                string blub = Split.Last();
                // Im Browser darstellen
                _Response += @"
                    <button><a href=Static.html?" + Split.Last() + ">" + Split.Last() + @"</a></button>
                    <br />
                ";
            }

            // An den Browser senden
            _Resp.ContentType = "text/html";
            _Resp.sendMessage(_sw, _Response);  
        }


        // ##########################################################################################################################################
        // Files öffnen - Im Browser auf einen Link gedrückt
        private void openFiles()
        {

            // Pfad am Server --> mit entsprechendem File
            string path = Environment.CurrentDirectory + "\\Files\\" + _Parameter[0];
            

            byte[] FileContent;
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                FileContent = new byte[fs.Length];
                fs.Read(FileContent, 0, Convert.ToInt32(fs.Length));
            }


            // Dateiendung für den MimeType bestimmen
            string[] FileExtension = _Parameter[0].Split('.');

            // Entsprechenden MimeType auswählen
            Extensions Extension = new Extensions();
            _ContentType = Extension.checkExtensions(FileExtension[1]);


            // An den Browser schicken
            _Resp.sendMessage(_sw, FileContent, _ContentType, _Parameter[0]);
            
        }
    }   
}


