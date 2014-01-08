using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class WrongPluginException : Exception
    {
        public WrongPluginException()
            : base()
        {
        }

        public WrongPluginException(string message)
            : base(message)
        {
        }

        public WrongPluginException(string message, Exception exception)
            : base(message, exception)
        {
        }
    }
}
