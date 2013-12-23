using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace Server
{
    public class DBHandler
    {
        private Dictionary<string, string> _data = new Dictionary<string, string>();
        private SqlConnection _db;

        public DBHandler()
        {
            _data.Add("Username", ".\\SqlExpress");
            _data.Add("Database", "SWE_Temperatur");
            _data.Add("Security", "true");

            _db = new SqlConnection("Data Source=" + _data["Username"] + "; Initial Catalog=" + _data["Database"] + "; Integrated Security=" + _data["Security"] + ";");
        }

        public SqlConnection Connection
        {
            get { return _db; }
        }

        public string Username
        {
            get { return _data["Username"]; }
        }

        public string Database
        {
            get { return _data["Database"]; }
        }

        public string Security
        {
            get { return _data["Security"]; }
        }
    }
}
