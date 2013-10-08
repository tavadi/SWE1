using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Server
{
    public class CTemp
    {

        private Random _rnd;
        private float _number;

        public CTemp()
        {
            Timer timer;
            timer = new Timer();
            timer.Interval = 500; //set interval of checking here
            timer.Elapsed += new ElapsedEventHandler(DoSomething);
            timer.Start();
        }

        void DoSomething(object sender, ElapsedEventArgs e)
        {
            _rnd = new Random();
            _number = _rnd.Next(100, 300);

            Console.WriteLine("{0:0.00}", (_number / 10));
        }


        public float Value
        {
            get
            {

                return 0;
            }

            set
            {
            }
        }
    }
}
