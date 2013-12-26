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

        private string _pluginName = "Static.html";
        private bool _isPlugin = false;

        private string[] _parameter;
        private string _response;

        private string[] _fileExtension;

        private StreamWriter _sw;

        // other classes
        private Response _resp = new Response();
        

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
                Console.WriteLine("ICH BIN DAS PLUGIN: " + _pluginName);
                return _pluginName;
            }
        }


        // ##########################################################################################################################################
        public bool IsPlugin
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
        private string[] Parameter
        {
            set
            {
                // Parameter werden übergeben
                _parameter = value;
                //Menu();
            }
        }


        // ##########################################################################################################################################
        public void Init(string[] parameter)
        {
            Parameter = parameter;
        }


        // ##########################################################################################################################################
        public void Run()
        {
            // Erster Aufruf --> Übersichtsseite
            if (_parameter[0] == _pluginName)
            {
                ShowFiles();
            }

            // Weitere Aufrufe --> Fileaufruf
            else
            {
                OpenFiles();
            }
        }



        // ##########################################################################################################################################
        private void ShowFiles()
        {
            // Pfad am Server --> gesamter Inhalt wird ausgegeben
            string path = Environment.CurrentDirectory + "\\Files\\";

            // Für jedes File
            foreach (string file in System.IO.Directory.GetFiles(path, "*"))
            {
                // Pfad wird gesplittet --> nur der Filename inkl. Endung wird benötigt
                IList<string> split = file.Split('\\');

                string blub = split.Last();
                // Im Browser darstellen
                _response += @"
                    <button><a href=Static.html?" + split.Last() + ">" + split.Last() + @"</a></button>
                    <br />
                ";
            }

            // An den Browser senden
            _resp.ContentType = "text/html";
            _resp.SendMessage(_sw, _response);
        }


        // ##########################################################################################################################################
        // Files öffnen - Im Browser auf einen Link gedrückt
        private void OpenFiles()
        {
            try
            {
                // Pfad am Server --> mit entsprechendem File
                string path = Environment.CurrentDirectory + "\\Files\\" + _parameter[0];


                byte[] fileContent;
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    fileContent = new byte[fs.Length];
                    fs.Read(fileContent, 0, Convert.ToInt32(fs.Length));
                }

                // Dateiname und Endung aufsplitten
                Split();

                // Entsprechenden MimeType auswählen
                Extensions extension = new Extensions();
                _resp.ContentType = extension.checkExtensions(_fileExtension[1]);


                // An den Browser schicken
                _resp.SendMessage(_sw, fileContent, _parameter[0]);
            }
            catch (FileNotFoundException e)
            {
                throw new WrongParameterException("Static ", e);
            }
        }


        public void Split()
        {
            // Dateiendung für den MimeType bestimmen
            _fileExtension = _parameter[0].Split('.');
        }


        // ##########################################################################################################################################
        public string FileExtension
        {
            get { return _fileExtension[1]; }
        }
    }
}


