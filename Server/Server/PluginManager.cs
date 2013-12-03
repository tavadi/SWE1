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

        private StreamWriter _sw;


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
                    //ERROR
                    continue;
                }
                
                // TO-DO
                // staticInstance. ...

                /*
                // Functions in each Plugin --> Interface
                PropertyInfo pluginName = type.GetProperty("PluginName");
                PropertyInfo isPlugin = type.GetProperty("IsPlugin");
                PropertyInfo writer = type.GetProperty("Writer");
                MethodInfo run = type.GetMethod("Run");
                MethodInfo init = type.GetMethod("Init");
                */

                
                // Call function pluginName and return value
                //string value = (string)pluginName.GetValue(staticInstance, null);

                filename = filename + ".html";
                // If filename is the same as pluginName in URL --> Set true
                if (filename == _name)
                {
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
                    catch (Exception e)
                    {
                        //Console.WriteLine("ERROR: " + e);
                        Console.WriteLine("ERROR: PluginManager - Run");
                    }
                }


                // Write true or false
                //Console.ForegroundColor = ConsoleColor.Yellow;
                //Console.WriteLine((bool)isPlugin.GetValue(StaticInstance, null));
                //Console.ForegroundColor = ConsoleColor.Green;
            }
        }  
    }
}
