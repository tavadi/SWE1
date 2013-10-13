using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public interface IPlugins
    {

        string PluginName
        {
            get;
        }

        bool isPlugin
        {
            get;
            set;
        }

        IList<string> doSomething
        {
            set;
            get;
        }
    }
}
