using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Address
    {
        private string _city;
        private string _street;


        public string City
        {
            set { _city = value; }
            get { return _city; }
        }

        public string Street
        {
            set { _street = value; }
            get { return _street; }
        }

    }
}
