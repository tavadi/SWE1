using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Xml;
using System.Web;
using Server;


namespace Server
{
    public class Navi : IPlugins
    {

        private string _PluginName = "Navi.html";
        private bool _isPlugin = false;
        private static bool _isPreparing = true;

        private string[] _Parameter;
        private string _Response;

        private StreamWriter _sw;
        private Response _Resp = new Response();

        private static Dictionary<string, List<string>> _StreetCity = new Dictionary<string, List<string>>();

        //private int counter = 0;
        //private int counter2 = 0;


        // ##########################################################################################################################################
        // Streamwriter
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
        public bool isPreparing
        {
            set
            {
                _isPreparing = value;
            }

            get
            {
                return _isPreparing;
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
        public void showMenu()
        {
            if (_Parameter == null)
            {
                _isPreparing = true;

                Thread thread = new Thread(Preparing);
                thread.Start();
            }

            else if (_Parameter[0] == _PluginName)
            {
                displayForm();
            }

            else if (_Parameter[0] == "Preparing")
            {
                if (_isPreparing == false)
                {
                    _isPreparing = true;

                    Thread thread = new Thread(Preparing);
                    thread.Start();

                    _Response = "Bitte warten Sie, w&auml;hrend die Daten aktualisiert werden";
                }
                else
                {
                    _Response = "Die Daten werden gerade aktualisiert. Bitte probieren Sie es sp&auml;ter erneut!";
                }
            }

            else if (_Parameter[0] == "Navigation")
            {
                _Response += @"
                    <form method=""POST"" action=""Navi.html?NavigationSearch"">
                        <label>Strassenname</label>
                        <input type=""text"" name=""street"" value=""Barrett Lane"" />
                        </br>
                        <input type=""submit"" value=""Submit"" />
                    </form>
                ";
            }

            else if (_Parameter[0] == "street")
            {
                if (_isPreparing == false)
                {
                    string DecodedParameter = _Parameter[1].Replace("+", " ");

                    string DecodedParam = HttpUtility.UrlDecode(DecodedParameter);


                    foreach (KeyValuePair<string, List<string>> kvp in _StreetCity)
                    {
                        try
                        {
                            if (kvp.Key == DecodedParam)
                            {
                                int size = kvp.Value.Count;

                                for (int i = 0; i < size; i++)
                                {
                                    //Console.WriteLine(kvp.Value[i]);
                                    _Response += kvp.Value[i];
                                    _Response += "<br />";
                                }
                            }
                        }
                        catch
                        {
                            _Response = "Kein Eintrag vorhanden";
                        }
                    }
                }
                else
                {
                    _Response = "Die Daten werden gerade aktualisiert. Bitte probieren Sie es sp&auml;ter erneut!";
                }
            }


            _Resp.ContentType = "text/html";
            _Resp.sendMessage(_sw, _Response);
        }


        // ##########################################################################################################################################
        private void Preparing()
        {
            DateTime date1 = DateTime.Now;
            Console.WriteLine(date1.ToString());

            Console.Beep();
            // Pfad am Server --> gesamter Inhalt wird ausgegeben
            string path = "C:\\Ress\\antarctica-latest.osm";
            Console.WriteLine(path);

            //counter = CountLines(path);
            //Console.WriteLine("Gesamtanzahl der Zeilen im File: " + counter);

            using (var fs = File.OpenRead(path))
            {
                using (var xml = new XmlTextReader(fs))
                {
                    while (xml.Read())
                    {
                        if (xml.NodeType == System.Xml.XmlNodeType.Element && xml.Name == "osm")
                        {
                            ReadOsm(xml);
                        }
                    }
                }
            }


            Console.Beep();

            DateTime date2 = DateTime.Now;
            Console.WriteLine(date2.ToString());
            Console.WriteLine(date2 - date1);
            Console.WriteLine("============================================done============================================");

            _Response = "READY";

            _isPreparing = false;


        }



        // ##########################################################################################################################################
        private void displayForm()
        {
            _Response =
               @"
                <html>
                    <head> 
                        <title>SensorCloud</title> 
                    </head> 
                    <body>
                        <button><a href=""Navi.html?Preparing"">Stra&szlig;enkarte neu aufbereiten</a></button>
                        <br />
                        <button><a href=""Navi.html?Navigation"">Stra&szlig;en <--> Orte</a></button>
                    </body> 
                </html>";

            _Resp.sendMessage(_sw, _Response);

        }


        // ##########################################################################################################################################
        private void ReadOsm(System.Xml.XmlTextReader xml)
        {
            using (var osm = xml.ReadSubtree())
            {
                while (osm.Read())
                {
                    if (osm.NodeType == System.Xml.XmlNodeType.Element && (osm.Name == "node" || osm.Name == "way"))
                    {
                        ReadAnyOsmElement(osm);
                    }
                }
            }
        }


        // ##########################################################################################################################################
        private void ReadAnyOsmElement(System.Xml.XmlReader osm)
        {
            Address a = new Address();
            string street = null;
            string city = null;

            using (var element = osm.ReadSubtree())
            {
                while (element.Read())
                {
                    if (element.NodeType == System.Xml.XmlNodeType.Element && element.Name == "tag")
                    {
                        ReadTag(element, a, city, street);
                    }
                }
            }
        }


        // ##########################################################################################################################################
        private void ReadTag(System.Xml.XmlReader element, Address a, string city, string street)
        {

            string tagType = element.GetAttribute("k");
            string value = element.GetAttribute("v");
            switch (tagType)
            {
                case "addr:city":
                    a.City = value;
                    break;

                case "addr:street":
                    a.Street = value;
                    break;
            }
            
            if (a.Street != null & a.City != null)
            {

                if (_StreetCity.ContainsKey(a.Street))
                {
                    if (!(_StreetCity[a.Street].Contains(a.City)))
                    {
                        _StreetCity[a.Street].Add(a.City);
                    }
                }
                else
                {
                    _StreetCity.Add(a.Street, new List<string>() { a.City });
                }
            }           
        }



        /*
        private static int CountLines(string fileToCount)
        {
            int counter = 0;
            using (StreamReader countReader = new StreamReader(fileToCount))
            {
                while (countReader.ReadLine() != null)
                    counter++;
            }
            return counter;
        }
        */

    }



    public class Address
    {
        private string _City;
        private string _Street;

        
        public string City
        {
            set
            {
                _City = value;
            }

            get
            {
                return _City;
            }
        }

        public string Street
        {
            set
            {
                _Street = value;
            }

            get
            {
                return _Street;
            }
        }

    }
}
