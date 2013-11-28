﻿using System;
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
    public class GetTemperatur : IPlugins
    {

        private string _PluginName = "GetTemperatur.html";
        private bool _isPlugin = false;
        private string[] _Parameter;
        private string _Response;

        private uint _Year;
        private uint _Month;
        private uint _Day;
        private uint _Max;

        private StreamWriter _sw;
        private Response _Resp = new Response();


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
                Console.WriteLine("ICH BIN DAS PLUGIN: " + _PluginName);
                return _PluginName;
            }
        }


        // ##########################################################################################################################################
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



        // ##########################################################################################################################################
        public string[] doSomething
        {
            set
            {
                _Parameter = value;
                showMenu();
            }
        }




        // ##########################################################################################################################################
        private void showMenu()
        {
            // Es wird in einem Thread ständig ein "Sensor" ausgelesen und in die Datenbank gespeichert
            // dieser Thread wird gestartet, wenn der Server gestartet wird
            if (_Parameter == null)
            {
                //Console.WriteLine("Es werden Sensordaten in die Datenbank gespeicher");
                readSensor Sensor = new readSensor();

                Thread thread = new Thread(Sensor.insertData);
                thread.Start();
            }

            // Es wurden keine Parameter übergeben
            else if (_Parameter.Length < 2)
            {
                // Anzeigen der Form
                if (_Parameter[0] == _PluginName)
                {
                    displayForm();
                }
            }

            // Es wurden Paramter übergeben --> Parameter aufbereiten und Ergebnisse ausgeben
            else
            {
                createResponse();
            }

        }




        // ##########################################################################################################################################
        private void displayForm()
        {
            _Response += @"
                <form method=""POST"" action=""GetTemperatur.html"">
                    <label>Year</label>
                    <input type=""text"" name=""year"" value=""2013"" />
                    </br>
                    <label>Month</label>
                    <input type=""text"" name=""month"" value=""10"" />
                    </br>
                    <label>Day</label>
                    <input type=""text"" name=""day"" value=""25"" />
                    </br>
                    <label>max. Eintr&auml;ge pro Seite</label>
                    <input type=""text"" name=""max"" value=""30"" />
                    </br>
                    <input type=""submit"" value=""Submit"" />
                </form>
            ";

            _Resp.ContentType = "text/html";
            _Resp.sendMessage(_sw, _Response);

        }





        // ##########################################################################################################################################
        private void createResponse()
        {
            // REST-Abfrage --> XML
            if (_Parameter.Length == 3)
            {
                _Year = Convert.ToUInt32(_Parameter[0]);
                _Month = Convert.ToUInt32(_Parameter[1]);
                _Day = Convert.ToUInt32(_Parameter[2]);

                // XML-File erstellen
                createXML XML = new createXML();
                _Response = XML.Create(_Year, _Month, _Day);

                _Resp.ContentType = "text/xml";
            }

            // Abfrage über Form
            else if (_Parameter.Length == 8)
            {
                int a = 0;

                // Parameter und Werte suchen
                for (int i = 0; i < _Parameter.Length; i++)
                {
                    a = i;
                    if (_Parameter[i] == "year")
                    {
                        _Year = Convert.ToUInt32(_Parameter[a + 1]);
                    }
                    else if (_Parameter[i] == "month")
                    {
                        _Month = Convert.ToUInt32(_Parameter[a + 1]);
                    }
                    else if (_Parameter[i] == "day")
                    {
                        _Day = Convert.ToUInt32(_Parameter[a + 1]);
                    }
                    else if (_Parameter[i] == "max")
                    {
                        _Max = Convert.ToUInt32(_Parameter[a + 1]);
                    }
                }


                if ((_Year.ToString().Length == 4) &&
                        ((_Month >= 1) & (_Month <= 12)) &&
                        ((_Day >= 1) & (_Day <= 31))
                   )
                {

                    // Datenbank 
                    //              Database:   SEW_Temperatur
                    //              Uuser:      local

                    using (SqlConnection db = new SqlConnection(
                        @"Data Source=.\SqlExpress;
                    Initial Catalog=SWE_Temperatur;
	                Integrated Security=true;"))
                    {
                        db.Open();

                        // SELECT
                        SqlCommand cmd = new SqlCommand("SELECT [DATE], [TEMPERATUR] FROM [MESSDATEN]", db);


                        int counter = 1;        // Zählt die Einträge pro Seite (max. _Max)
                        int groupcounter = 1;   // zählt die Gruppen zu je _Max Einträgen

                        // Es werden Ergebnisse gesucht und wenn gefunden, in <div>'s eingetragen
                        using (SqlDataReader rd = cmd.ExecuteReader())
                        {
                            _Response += "<div id='container'>";
                            while (rd.Read())
                            {
                                //Console.Write(rd.GetDateTime(0).Date.Year + " - ");
                                //Console.Write(rd.GetDateTime(0).Date.Month + " - ");
                                //Console.WriteLine(rd.GetDateTime(0).Date.Day);

                                if ((rd.GetDateTime(0).Date.Year.ToString() == _Year.ToString()) &&
                                    (rd.GetDateTime(0).Date.Month.ToString() == _Month.ToString()) &&
                                    (rd.GetDateTime(0).Date.Day.ToString() == _Day.ToString()))
                                {

                                    // Ausgabe wird für die Blätterfunktion gruppiert (_Max Elemente = 1 Gruppe) 
                                    if (counter == 1)
                                    {
                                        _Response += "<div id='group" + groupcounter + "' class='group'>";
                                        groupcounter++;
                                    }
                                    counter++;

                                    // Inhalt aus der DB
                                    _Response += "<div class='line'><div class='min20'>" + rd.GetDateTime(0).ToString() + "</div><div class='min20'>" + rd.GetDecimal(1).ToString() + "</div></div>";

                                    // Es werden maximal _Max Ergebnisse pro "Seite" ausgegeben --> Blätterfunktion
                                    if (counter == _Max)
                                    {
                                        _Response += "</div>";
                                        counter = 1;
                                    }
                                }
                            }
                            _Response += "</div>";
                        }
                    }
                }

                else
                {
                    Console.WriteLine("ERRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRROR");
                    _Response = "Error - Bitte ein korrektes Datum eingeben!s";
                    throw new ArgumentOutOfRangeException("amount");
                }

                _Resp.ContentType = "text/html";
            }

            // Zum Browser senden
            _Resp.sendMessage(_sw, _Response);
        }
    }
}
