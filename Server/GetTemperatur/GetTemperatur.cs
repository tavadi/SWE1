﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Server;

namespace Server
{
    public class GetTemperatur : IPlugins
    {

        private string _PluginName = "GetTemperatur.html";
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


                if (_Parameter.Count < 2)
                {
                    _Response.Add(  @"
                                    <form method=""POST"" action=""GetTemperatur.html"">
                                        <label>Year</label>
                                        <input type=""text"" name=""year"" value=""2012"" />
                                        </br>
                                        <label>Month</label>
                                        <input type=""text"" name=""month"" value=""01"" />
                                        </br>
                                        <label>Day</label>
                                        <input type=""text"" name=""day"" value=""02"" />
                                        </br>
                                        <input type=""submit"" value=""Submit"" />
                                    </form>
                                ");
                }


                int a = 0;

                _Response.Add("<h1>");
                for (int i = 0; i < _Parameter.Count; i++)
                {
                    a = i;
                    if (_Parameter[i] == "year")
                    {
                        _Response.Add("JAHR: " + _Parameter[a + 1]);
                    }
                    else if (_Parameter[i] == "month")
                    {
                        _Response.Add("MONAT: " + _Parameter[a + 1]);
                    }
                    else if (_Parameter[i] == "day")
                    {
                        _Response.Add("TAG: " + _Parameter[a + 1]);
                    }
                }
                _Response.Add("</h1>");
                
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
