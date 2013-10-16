using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;


namespace Server
{
    class CPluginManager
    {
        
        private string _Name;
        private IList<string> _Parameter;
        private IList<string> _Response;

        // Konstruktor
        public CPluginManager(string PluginName, IList<string> URL)
        {
            _Name = PluginName;
            _Parameter = URL;
            checkPlugin();
        }
        
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

                // Server.CGetTemperatur
                // Server.CStatic
                Type type = myDllAssembly.GetType("Server." + filename);

                // Create Instance 
                object StaticInstance = Activator.CreateInstance(type);


                // Functions in each Plugin --> Interface
                PropertyInfo PluginName = type.GetProperty("PluginName");
                PropertyInfo isPlugin = type.GetProperty("isPlugin");
                PropertyInfo doSomething = type.GetProperty("doSomething");

                
                //object blub = StaticInstance.GetType().GetProperties();

                
                // Call function PluginName and return value
                string value = (string)PluginName.GetValue(StaticInstance, null);

                // If filename is the same as PluginName in URL --> Set true
                if (filename == _Name)
                {
                    isPlugin.SetValue(StaticInstance, true, null);
                    doSomething.SetValue(StaticInstance, _Parameter, null);

                    _Response = (IList<string>)doSomething.GetValue(StaticInstance, null);
                }

                

                // Write true or false
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine((bool)isPlugin.GetValue(StaticInstance, null));
                Console.ForegroundColor = ConsoleColor.Green;

                
            }
        }

        public IList<string> Response
        {
            get
            {
                return _Response;
            }
        }
        
    }
}
