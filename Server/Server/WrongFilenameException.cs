using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class WrongFilenameException : Exception
    {
        public WrongFilenameException()
            : base()
        {
        }

        public WrongFilenameException(string message)
            : base(message)
        {
        }

        public WrongFilenameException(string message, Exception exception)
            : base(message, exception)
        {
        }
    }
}
