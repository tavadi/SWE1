using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;


namespace Server
{
    public class PluginManager
    {
        
        private string _name;
        private string[] _parameter;
        private bool _firstAccess;

        private bool _pluginExists = false;

        private StreamWriter _sw;

        // other classes
        private Response _response = new Response();

        private LogFile _logFile;

        // ##########################################################################################################################################
        // Konstruktor
        public PluginManager(string pluginName, bool firstAccess)
        {
            _name = pluginName;
            _firstAccess = firstAccess;

            CheckPlugin();
        }


        // ##########################################################################################################################################
        public PluginManager(string pluginName, string[] parameter, StreamWriter sw)
        {
            _name = pluginName;
            _parameter = parameter;

            _sw = sw;

            CheckPlugin();
        }


        // ##########################################################################################################################################
        public void CheckPlugin()
        {
            // Navigate to Path (DLL-Files from Plugins)
            string path = Environment.CurrentDirectory + "\\Plugins\\";


            // For each DLL-File
            foreach (string plugin in System.IO.Directory.GetFiles(path, "*.dll"))
            {
                System.Reflection.Assembly myDllAssembly = System.Reflection.Assembly.LoadFile(plugin);

                //filter FILENAME without Extension
                string filename = Path.GetFileNameWithoutExtension(plugin);
                //Console.WriteLine(result);

                // Server.Temperatur
                // Server.Static
                Type type = myDllAssembly.GetType("Server." + filename);

                // Create Instance 
                IPlugins staticInstance = null;
                try
                {
                    staticInstance = Activator.CreateInstance(type) as IPlugins;
                }
                catch (ArgumentNullException e)
                {
                    // Tritt auf, wenn eine dll-Datei kopiert wurde und unter einem anderen Namen abgespeichert ist
                    // Pluginname und Dateiname passen nicht zusammen
                    //Console.WriteLine("PluginManager: Es ist ein Fehler im folgenden Plugin aufgetreten: " + filename);
                    _logFile = new LogFile(e.ToString());

                }
                
                if (staticInstance == null)
                {
                    continue;
                }


                // Startseite anzeigen
                if (_name == "")
                {
                    _pluginExists = true;
                }
                else
                {
                    filename += ".html";

                    // If filename is the same as pluginName in URL --> Set true
                    if (filename == _name)
                    {
                        _pluginExists = true;

                        staticInstance.IsPlugin = true;
                        staticInstance.Writer = _sw;

                        try
                        {
                            object[] param = { _parameter };

                            staticInstance.Init(_parameter);
                            staticInstance.Run();
                        }

                        catch (WrongParameterException e)
                        {
                            Console.WriteLine("Falscher Parameter: 404 Error");

                            // 404 ErrorPage
                            _response.ContentType = "text/html";
                            _response.Status = false;
                            _response.SendMessage(_sw, "Bitte &uuml;berpr&uuml;fen Sie Ihre Paramter.");

                            /*
                            if (e.InnerException != null)
                            {
                                Console.WriteLine(e.InnerException.StackTrace);
                            }
                            */

                            _logFile = new LogFile(e.ToString());
                        }

                        catch (WrongFilenameException e)
                        {
                            Console.WriteLine("Falscher Parameter: 404 Error");

                            // 404 ErrorPage
                            _response.ContentType = "text/html";
                            _response.Status = false;
                            _response.SendMessage(_sw, "Bitte &uuml;berpr&uuml;fen Sie die URL von diesem Feed.");

                            /*
                            if (e.InnerException != null)
                            {
                                Console.WriteLine(e.InnerException.StackTrace);
                            }
                            */

                            _logFile = new LogFile(e.ToString());
                        }

                        catch (Exception e)
                        {
                            //Console.WriteLine("ERROR: " + e);
                            _response.ContentType = "text/html";
                            _response.Status = false;
                            _response.SendMessage(_sw, "Error PluginManager - " + e);

                            _logFile = new LogFile(e.ToString());
                        }
                    }
                }

                // Write true or false
                //Console.ForegroundColor = ConsoleColor.Yellow;
                //Console.WriteLine((bool)isPlugin.GetValue(StaticInstance, null));
                //Console.ForegroundColor = ConsoleColor.Green;
            }

            if ((_firstAccess == false) && (_pluginExists == false))
            {
                _response.ContentType = "text/html";
                _response.SendMessage(_sw, "Error PluginManager - Ihr ausgew&auml;hltes Plugin ist derzeit nicht verf&uuml;gbar. Bitte probieren Sie es sp&auml;ter erneut");
            }
        }  
    }
}
