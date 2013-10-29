﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Timers;
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
        private IList<string> _Parameter;
        private IList<string> _Response;

        private string _Year;
        private string _Month;
        private string _Day;
        private string _Max;

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

                // Es wurden keine Parameter übergeben
                if (_Parameter.Count < 2)
                {

                    // Es sollen Daten aus dem Sensor ausgelesen und in die Datenbank gespeichert werden
                    if (_Parameter[0] == "Sensor")
                    {
                        // Ließt ständig Daten aus
                        Thread thread = new Thread(insertData);
                        thread.Start();

                        _Response.Add("<h1>");
                        _Response.Add("Es werden Daten aus dem Sensor ausgelesen und in die Datenbank gespeichert.");
                        _Response.Add("</h1>");
                    }

                    // Anzeigen der Form
                    else if (_Parameter[0] == "Messwerte")
                    {
                        displayForm();
                    }

                    // Anzeigen aller Möglichkeiten
                    else if (_Parameter[0] == _PluginName)
                    {
                        _Response.Add(@"
                            <button><a href=""GetTemperatur.html?Sensor"">Messwerte aus Sensor auslesen</a></button>
                            <br />
                            <button><a href=""GetTemperatur.html?Messwerte"">Messwerte filtern</a></button>
                        ");
                    }

                }
                else
                {
                    // Parameter wurden übergeben
                    // Response erstellen
                    displayForm();
                    createResponse();
                }

                return _Response;
            }
        }


        private void displayForm()
        {
            _Response.Add(@"
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
                    <label>Maximal</label>
                    <input type=""text"" name=""max"" value="""" />
                    </br>
                    <input type=""submit"" value=""Submit"" />
                </form>
            ");
        }


        private void createResponse()
        {
            int counter = 1;
            int groupcounter = 1;

            int a = 0;

            // REST-Abfrage --> XML
            if (_Parameter.Count == 3)
            {
                _Year = _Parameter[0];
                _Month = _Parameter[1];
                _Day = _Parameter[2];

                // XML-File erstellen
                createXML();
                
            }

            // Abfrage über Form
            else if (_Parameter.Count == 8)
            {
                for (int i = 0; i < _Parameter.Count; i++)
                {
                    a = i;
                    if (_Parameter[i] == "year")
                    {
                        _Year = _Parameter[a + 1];
                    }
                    else if (_Parameter[i] == "month")
                    {
                        _Month = _Parameter[a + 1];
                    }
                    else if (_Parameter[i] == "day")
                    {
                        _Day = _Parameter[a + 1];
                    }
                    else if (_Parameter[i] == "max")
                    {
                        _Max = _Parameter[a + 1];
                    }
                }
            }


            using (SqlConnection db = new SqlConnection(
                @"Data Source=.\SqlExpress;
                Initial Catalog=SWE_Temperatur;
	            Integrated Security=true;"))
            {
                db.Open();


                SqlCommand cmd = new SqlCommand("SELECT [DATE], [TEMPERATUR] FROM [MESSDATEN]", db);

                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    _Response.Add("<div id='container'>");
                    while (rd.Read())
                    {
                        //Console.Write(rd.GetDateTime(0).Date.Year + " - ");
                        //Console.Write(rd.GetDateTime(0).Date.Month + " - ");
                        //Console.WriteLine(rd.GetDateTime(0).Date.Day);

                        if ((rd.GetDateTime(0).Date.Year.ToString() == _Year) &&
                            (rd.GetDateTime(0).Date.Month.ToString() == _Month) &&
                            (rd.GetDateTime(0).Date.Day.ToString() == _Day))
                        {

                            // Ausgabe wird für die Blätterfunktion gruppiert (30 Elemente --> 1 Gruppe) 
                            if (counter == 1)
                            {
                                _Response.Add("<div id='group" + groupcounter + "' class='group'>");
                                groupcounter++;
                            }
                            counter++;

                            // Inhalt aus der DB
                            _Response.Add("<div class='line'><div class='min20'>" + rd.GetDateTime(0).ToString() + "</div><div class='min20'>" + rd.GetDecimal(1).ToString() + "</div></div>");
                            
                            if (counter == 30)
                            {
                                _Response.Add("</div>");
                                counter = 1;
                            }
                        }
                    } 
                    _Response.Add("</div>");
                }
             }
        }



        private void insertData()
        {
            /*
            System.Timers.Timer timer;
            timer = new System.Timers.Timer();
            timer.Interval = 1000; //set interval of checking here
            timer.Elapsed += new ElapsedEventHandler(Insert);
            timer.Start();
            */

            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Elapsed += new ElapsedEventHandler(Insert);
            timer.Interval = 1000;
            timer.Enabled = true;

        }

        private void Insert(object source, ElapsedEventArgs e)
        {
            Random rnd = new Random();
            float number = rnd.Next(100, 300);

            float Temp = (number / 10);
            
            Console.WriteLine("{0:0.00}", Temp);


            using (SqlConnection db = new SqlConnection(@"Data Source=.\SqlExpress;Initial Catalog=SWE_Temperatur; Integrated Security=true;"))
            {
                db.Open();

                // Komma durch Punkt ersetzen, weil der Wert in der DB als x.xx gespeichert werden muss
                string str = Convert.ToString(Temp);
                str = str.Replace(",", ".");

                SqlCommand insert = new SqlCommand(@"INSERT INTO [MESSDATEN] ([DATE], [TEMPERATUR], [TIMESTAMP]) VALUES (getdate(), " + str + ", getdate())", db);
                insert.ExecuteNonQuery();

                db.Close();

            }
        }


        private void createXML()
        {
            string date = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_ffff");
            string file = Directory.GetCurrentDirectory() + "/XML/TemperatureXML_" + date + ".xml";

            if (File.Exists(file))
            {
                File.Delete(file);
            }
            
            // Create a file to write to.
            using (StreamWriter sw = new StreamWriter(file))
            {
                sw.WriteLine("<?xml version='1.0' encoding='UTF-8' standalone='yes'?>");
                sw.WriteLine("<PluginTemperatur>");
                sw.WriteLine("<title>Plugin Temperatur</title>");


                using (SqlConnection db = new SqlConnection(
                    @"Data Source=.\SqlExpress;
                    Initial Catalog=SWE_Temperatur;
	                Integrated Security=true;"))
                {
                    db.Open();
                    SqlCommand cmd = new SqlCommand(@"SELECT [DATE], [TEMPERATUR] 
                                                FROM [MESSDATEN] 
                                                WHERE YEAR([DATE]) = '" + _Year + "' AND MONTH([DATE]) = '" + _Month + "' AND DAY([DATE]) = '" + _Day + "'", db);

                    using (SqlDataReader rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            sw.WriteLine("<element>");
                            sw.WriteLine("<Date>" + _Year + "." + _Month + "." + _Day + "</Date>");
                            sw.WriteLine("<Temperature>" + rd.GetDecimal(1).ToString() + "</Temperature>");
                            sw.WriteLine("</element>");

                        }
                    }
                }

                sw.WriteLine("</PluginTemperatur>");
            }
        }
    }
}
