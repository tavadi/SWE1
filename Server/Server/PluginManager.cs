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

        private bool _pluginExists = false;

        private StreamWriter _sw;
        private Response _resp = new Response();


        // ##########################################################################################################################################
        // Konstruktor
        public PluginManager(string pluginName)
        {
            _name = pluginName;
            CheckPlugin();
        }

/*
        // ##########################################################################################################################################
        public PluginManager(string pluginName, string[] Parameter)
        {
            _name = pluginName;
            _parameter = Parameter;

            checkPlugin();
        }
*/

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

                // Server.GetTemperatur
                // Server.Static
                Type type = myDllAssembly.GetType("Server." + filename);

                // Create Instance 
                IPlugins staticInstance = Activator.CreateInstance(type) as IPlugins;

                
                if (staticInstance == null)
                {
                    continue;
                }
                

                // Call function pluginName and return value
                //string value = (string)pluginName.GetValue(staticInstance, null);

                // Startseiete
                if (_name == "")
                {
                    _pluginExists = true;
                }
                else
                {
                    filename = filename + ".html";
                    // If filename is the same as pluginName in URL --> Set true
                    if (filename == _name)
                    {
                        _pluginExists = true;

                        //isPlugin.SetValue(staticInstance, true, null);
                        //writer.SetValue(staticInstance, _sw, null);

                        staticInstance.IsPlugin = true;
                        staticInstance.Writer = _sw;

                        try
                        {
                            object[] param = { _parameter };

                            //init.Invoke(staticInstance, param);
                            //run.Invoke(staticInstance, null);

                            staticInstance.Init(_parameter);
                            staticInstance.Run();
                        }

                        catch (WrongParameterException e)
                        {
                            Console.WriteLine("Falscher Parameter: 404 Error");

                            // 404 ErrorPage
                            _resp.ContentType = "text/html";
                            _resp.Status = false;
                            _resp.SendMessage(_sw, "Bitte &uuml;berpr&uuml;fen Sie Ihre Paramter.");

                            if (e.InnerException != null)
                            {
                                Console.WriteLine(e.InnerException.StackTrace);
                            }
                        }

                        catch (Exception e)
                        {
                            //Console.WriteLine("ERROR: " + e);
                            _resp.ContentType = "text/html";
                            _resp.Status = false;
                            _resp.SendMessage(_sw, "Error PluginManager - " + e);
                        }
                    }
                }

                // Write true or false
                //Console.ForegroundColor = ConsoleColor.Yellow;
                //Console.WriteLine((bool)isPlugin.GetValue(StaticInstance, null));
                //Console.ForegroundColor = ConsoleColor.Green;
            }

            if (_pluginExists == false)
            {
                _resp.ContentType = "text/html";
                _resp.SendMessage(_sw, "Error PluginManager - Ihr ausgew&auml;hltes Plugin ist derzeit nicht verf&uuml;gbar. Bitte probieren Sie es sp&auml;ter erneut");
            }
        }  
    }
}
