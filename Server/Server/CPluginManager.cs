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
        
        private string _PluginName;
        private IList<string> _Plugins;
        //private bool _PluginExists = false;


        // Konstruktor
        public CPluginManager(string PluginName)
        {
            _PluginName = PluginName;
            checkPlugin();
        }
        
        public void checkPlugin()
        {
            string path = Environment.CurrentDirectory + "\\Plugins\\";

            foreach (string Plugin in System.IO.Directory.GetFiles(path, "*.dll"))
            {
                System.Reflection.Assembly myDllAssembly = System.Reflection.Assembly.LoadFile(Plugin);


                //FILENAME
                string result = Path.GetFileNameWithoutExtension(Plugin);
                //Console.WriteLine(result);

                Type type = myDllAssembly.GetType("Server." + result);


                object StaticInstance = Activator.CreateInstance(type);
                PropertyInfo CorrectPlugin = type.GetProperty("PluginName");
                PropertyInfo isPlugin = type.GetProperty("isPlugin");

                string value = (string)CorrectPlugin.GetValue(StaticInstance, null);
                
                if (value == _PluginName)
                {
                    isPlugin.SetValue(StaticInstance, true, null);

                    /*
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("TRUE");
                    Console.ForegroundColor = ConsoleColor.Green;
                    */
                }
                else
                {
                    /*
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("FALSE");
                    Console.ForegroundColor = ConsoleColor.Green;
                    */
                }

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine((bool)isPlugin.GetValue(StaticInstance, null));
                Console.ForegroundColor = ConsoleColor.Green;

            }
        }
        



/*        public void checkPlugin(string PluginName, IList<string> _URL)
        {
            
            ReadPlugins();

            for (int i = 0; i < _Plugins.Count(); i++)
            {
                if (PluginName == _Plugins[i])
                {
                    Console.WriteLine("Plugin ist vorhanden " + PluginName);
                    _PluginExists = true;
                    break;
                }
            }

            if (_PluginExists == true)
            {
                
                switch (PluginName)
                {
                    case _Plugins[0]:
                        Console.WriteLine("Temp");
                        break;

                    case :
                        Console.WriteLine("Static");
                        break;

                    default:
                        Console.WriteLine("Wrong Plugin");
                        break;
                }
                
            }
       
        }
*/

        // ############################################################################################################
        // Read txt-File
        private void ReadPlugins()
        {
            string line;
            int counter = 0;

            _Plugins = new List<string>();


            StreamReader file = new StreamReader("../../Plugins.txt");

            while ((line = file.ReadLine()) != null)
            {
                //Console.WriteLine(line);
                _Plugins.Add(line);
                counter++;
            }

            file.Close();
        }

    }
}
