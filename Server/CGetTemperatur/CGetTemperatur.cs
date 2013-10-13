using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Server;

namespace Server
{
    public class CGetTemperatur : IPlugins
    {
        
        private string _PluginName = "CGetTemperatur";
        private bool _isPlugin = false;
        private IList<string> _Parameter;
        private IList<string> _Response;



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



        public IList<string> doSomething
        {
            set
            {
                _Parameter = value;
            }

            get
            {
                _Response = new List<string>();

                // Parameter[0] == Jahr
                // Parameter[1] == Monat
                // Parameter[2] == Tag
                
                _Response.Add("JAHR: " + _Parameter[0]);
                _Response.Add("MONAT: " + _Parameter[1]);
                _Response.Add("TAG: " + _Parameter[2]);

                return _Response;
            }
        }























        /*
        private Random _rnd;
        private float _number;

        public CStatic()
        {
            Timer timer;
            timer = new Timer();
            timer.Interval = 100; //set interval of checking here
            timer.Elapsed += new ElapsedEventHandler(DoSomething);
            timer.Start();
        }

        void DoSomething(object sender, ElapsedEventArgs e)
        {
            _rnd = new Random();
            _number = _rnd.Next(100, 300);

            //Console.WriteLine("{0:0.00}", (_number / 10));
        }
        */
    }
}
