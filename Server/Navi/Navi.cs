using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Server;

namespace Server
{
    public class Navi : IPlugins
    {


        private string _PluginName = "Navi.html";
        private bool _isPlugin = false;
        private IList<string> _Parameter;
        private IList<string> _Response;

        private string _ContentType;


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
            }

            get
            {

                return _Response;
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

    }
}
