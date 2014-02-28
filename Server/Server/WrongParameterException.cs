using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class WrongParameterException : Exception
    {
        public WrongParameterException() 
            : base()
        {
        }

        public WrongParameterException(string message)
            : base(message)
        {
        }

        public WrongParameterException(string message, Exception exception)
            : base(message, exception)
        {
        }
    }
}
