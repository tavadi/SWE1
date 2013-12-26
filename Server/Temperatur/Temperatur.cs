﻿﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;
using Server;

namespace Server
{
    public class Temperatur : IPlugins
    {
        private string _pluginName = "Temperatur.html";
        private bool _isPlugin = false;
        private string[] _parameter;
        private string _response;

        private uint _year;
        private uint _month;
        private uint _day;
        private uint _max;

        private StreamWriter _sw;

        // other classes
        private Response _resp = new Response();
        private DBHandler _dbHandler = new DBHandler();


        // ##########################################################################################################################################
        // StreamWriter
        public StreamWriter Writer
        {
            set
            {
                _sw = value;
            }
        }


        // ##########################################################################################################################################
        public string PluginName
        {
            get
            {
                Console.WriteLine("ICH BIN DAS PLUGIN: " + _pluginName);
                return _pluginName;
            }
        }


        // ##########################################################################################################################################
        public bool IsPlugin
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


        // ##########################################################################################################################################
        private string[] Parameter
        {
            set
            {
                _parameter = value;
            }
        }


        // ##########################################################################################################################################
        public void Init(string[] parameter)
        {
            _parameter = parameter;
        }




        // ##########################################################################################################################################
        public void Run()
        {
            // Es wird in einem Thread ständig ein "Sensor" ausgelesen und in die Datenbank gespeichert
            // dieser Thread wird gestartet, wenn der Server gestartet wird
            if (_parameter == null)
            {
                //Console.WriteLine("Es werden Sensordaten in die Datenbank gespeicher");
                SensorReader Sensor = new SensorReader();

                Thread thread = new Thread(Sensor.InsertData);
                thread.Start();
            }

            // Es wurden keine Parameter übergeben --> Anzeige Eingabeform
            else if (_parameter.Length < 2)
            {
                DisplayForm();
            }

            // Es wurden Paramter übergeben --> Parameter aufbereiten und Ergebnisse ausgeben
            else
            {
                PrepareResponse();
            }

        }




        // ##########################################################################################################################################
        private void DisplayForm()
        {
            _response += @"
                <form method='POST' action='Temperatur.html'>
                    <label>Year</label>
                    <input type='text' name='year' value='2012' />
                    </br>
                    <label>Month</label>
                    <input type='text' name='month' value='12' />
                    </br>
                    <label>Day</label>
                    <input type='text' name='day' value='13' />
                    </br>
                    <label>max. Eintr&auml;ge pro Seite</label>
                    <input type='text' name='max' value='30' />
                    <label>min: 10; max: 100</label>
                    </br>
                    <input type='submit' value='Submit' />
                </form>
            ";

            _resp.ContentType = "text/html";
            _resp.SendMessage(_sw, _response);

        }





        // ##########################################################################################################################################
        private void PrepareResponse()
        {
            // REST-Abfrage --> XML
            if (_parameter.Length == 3)
            {
                PrepareXML();
            }

            // Abfrage über Form
            else if (_parameter.Length == 8)
            {
                ParseParameters();
                PrepareAnswer();
            }

            // Zum Browser senden
            _resp.SendMessage(_sw, _response);
        }




        // ##########################################################################################################################################
        private void PrepareXML()
        {
            _year = Convert.ToUInt32(_parameter[0]);
            _month = Convert.ToUInt32(_parameter[1]);
            _day = Convert.ToUInt32(_parameter[2]);
            _max = 10;

            // Check Parameter
            checkParameter();

            // XML-File erstellen
            XmlCreator XML = new XmlCreator();
            _response = XML.Create(_year, _month, _day);

            _resp.ContentType = "text/xml";
        }



        // ##########################################################################################################################################
        public void ParseParameters()
        {
            int a = 0;

            try
            {
                // Parameter und Werte suchen
                for (int i = 0; i < _parameter.Length; ++i)
                {
                    a = i;
                    if (_parameter[i] == "year")
                    {
                        _year = Convert.ToUInt32(_parameter[a + 1]);
                    }
                    else if (_parameter[i] == "month")
                    {
                        _month = Convert.ToUInt32(_parameter[a + 1]);
                    }
                    else if (_parameter[i] == "day")
                    {
                        _day = Convert.ToUInt32(_parameter[a + 1]);
                    }
                    else if (_parameter[i] == "max")
                    {
                        _max = Convert.ToUInt32(_parameter[a + 1]);
                    }
                }

                // Check Parameter
                checkParameter();

            }
            catch (FormatException e)
            {
                throw new WrongParameterException("TemperaturPlugin ", e);
            }
        }


        // ##########################################################################################################################################
        private void checkParameter()
        {
            if (_year.ToString().Length != 4)
            {
                throw new WrongParameterException("TemperaturPlugin");
            }
            else if ((_day <= 0) || (_day > 31))
            {
                throw new WrongParameterException("TemperaturPlugin");
            }
            else if ((_month <= 0) || (_month > 12))
            {
                throw new WrongParameterException("TemperaturPlugin");
            }
            else if ((_max < 10) || (_max > 100))
            {
                throw new WrongParameterException("TemperaturPlugin");
            }

        }


        // ##########################################################################################################################################
        private void PrepareAnswer()
        {
            // Datenbank 
            //              Database:   SEW_Temperatur
            //              Uuser:      local

            SqlConnection db = _dbHandler.Connection;

            using (db)
            {
                db.Open();

                // SELECT
                SqlCommand cmd = new SqlCommand("SELECT [MESSDATEN].[DATE], [MESSDATEN].[TEMPERATUR] FROM [MESSDATEN]", db);


                int counter = 1;        // Zählt die Einträge pro Seite (max. _max)
                int groupcounter = 1;   // zählt die Gruppen zu je _max Einträgen

                // Es werden Ergebnisse gesucht und wenn gefunden, in <div>'s eingetragen
                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    _response += "<div id='container'>";
                    while (rd.Read())
                    {
                        //Console.Write(rd.GetDateTime(0).Date.Year + " - ");
                        //Console.Write(rd.GetDateTime(0).Date.Month + " - ");
                        //Console.WriteLine(rd.GetDateTime(0).Date.Day);

                        if ((rd.GetDateTime(0).Date.Year.ToString() == _year.ToString()) &&
                            (rd.GetDateTime(0).Date.Month.ToString() == _month.ToString()) &&
                            (rd.GetDateTime(0).Date.Day.ToString() == _day.ToString()))
                        {

                            // Ausgabe wird für die Blätterfunktion gruppiert (_max Elemente = 1 Gruppe) 
                            if (counter == 1)
                            {
                                _response += "<div id='group" + groupcounter + "' class='group'>";
                                groupcounter++;
                            }
                            counter++;

                            // Inhalt aus der DB
                            _response += "<div class='line'><div class='min20'>" + rd.GetDateTime(0).ToString() + "</div><div class='min20'>" + rd.GetDecimal(1).ToString() + "</div></div>";

                            // Es werden maximal _max Ergebnisse pro "Seite" ausgegeben --> Blätterfunktion
                            if (counter == _max)
                            {
                                _response += "</div>";
                                counter = 1;
                            }
                        }
                    }
                    _response += "</div>";
                }
            }

            _resp.ContentType = "text/html";
        }






        // ##########################################################################################################################################
        public uint Year
        {
            get { return _year; }
        }

        // ##########################################################################################################################################
        public uint Month
        {
            get { return _month; }
        }

        // ##########################################################################################################################################
        public uint Day
        {
            get { return _day; }
        }

        // ##########################################################################################################################################
        public uint Max
        {
            get { return _max; }
        }



        // ##########################################################################################################################################
        // Database Connection
        // ##########################################################################################################################################
        public string Username
        {
            get { return _dbHandler.Username; }
        }

        // ##########################################################################################################################################
        public string Database
        {
            get { return _dbHandler.Database; }
        }

        // ##########################################################################################################################################
        public string Security
        {
            get { return _dbHandler.Security; }
        }

    }


}
