using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;


namespace Server
{
    class PluginManager
    {
        
        private string _Name;
        private string _ContentType;
        private IList<string> _Parameter;
        private string _Response;

        private StreamWriter _sw;



        // ##########################################################################################################################################
        // Konstruktor
        public PluginManager(string PluginName)
        {
            _Name = PluginName;
            checkPlugin();
        }

        // ##########################################################################################################################################
        public PluginManager(string PluginName, IList<string> URL, StreamWriter sw)
        {
            _Name = PluginName;
            _Parameter = URL;

            _sw = sw;
            checkPlugin();
        }




        // ##########################################################################################################################################
        public void checkPlugin()
        {
            // Navigate to Path (DLL-Files from Plugins)
            string path = Environment.CurrentDirectory + "\\Plugins\\";

            // For each DLL-File
            foreach (string Plugin in System.IO.Directory.GetFiles(path, "*.dll"))
            {
                System.Reflection.Assembly myDllAssembly = System.Reflection.Assembly.LoadFile(Plugin);

                //filter FILENAME without Extension
                string filename = Path.GetFileNameWithoutExtension(Plugin);
                //Console.WriteLine(result);

                // Server.GetTemperatur
                // Server.Static
                Type type = myDllAssembly.GetType("Server." + filename);

                // Create Instance 
                object StaticInstance = Activator.CreateInstance(type);

                // Functions in each Plugin --> Interface
                PropertyInfo PluginName = type.GetProperty("PluginName");
                PropertyInfo isPlugin = type.GetProperty("isPlugin");
                PropertyInfo doSomething = type.GetProperty("doSomething");
                PropertyInfo ContentType = type.GetProperty("ContentType");
                PropertyInfo Writer = type.GetProperty("Writer");
                
                
                // Call function PluginName and return value
                string value = (string)PluginName.GetValue(StaticInstance, null);

                filename = filename + ".html";
                // If filename is the same as PluginName in URL --> Set true
                if (filename == _Name)
                {
                    isPlugin.SetValue(StaticInstance, true, null);
                    Writer.SetValue(StaticInstance, _sw, null);
                    doSomething.SetValue(StaticInstance, _Parameter, null);
                }


                // Write true or false
                //Console.ForegroundColor = ConsoleColor.Yellow;
                //Console.WriteLine((bool)isPlugin.GetValue(StaticInstance, null));
                //Console.ForegroundColor = ConsoleColor.Green;

            }
        }  
    }
}
