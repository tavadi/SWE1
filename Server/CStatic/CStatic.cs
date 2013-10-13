using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Server;

namespace Server
{
    public class CStatic : IPlugins
    {

        private string _PluginName = "CStatic";
        private bool _isPlugin = false;


        public string PluginName
        {
            get
            {
                Console.WriteLine("ICH BIN DAS PLUGIN: " + _PluginName);
                return _PluginName;
            }
        }


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


        public void doSomething()
        {

        }

    }
}
