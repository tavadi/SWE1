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

<<<<<<< HEAD

        public void checkPlugin()
        {
        }


=======
>>>>>>> 81910c174ba64d9bdc3151812c4470b13f00d0b1

        public string CorrectPlugin
        {
            get
            {
                Console.WriteLine("ICH BIN DAS PLUGIN: " + _PluginName);
                return _PluginName;
            }
        }

<<<<<<< HEAD
=======

>>>>>>> 81910c174ba64d9bdc3151812c4470b13f00d0b1
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

<<<<<<< HEAD

        public void doSomething()
        {

        }

=======
>>>>>>> 81910c174ba64d9bdc3151812c4470b13f00d0b1
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
