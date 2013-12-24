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

        private string _pluginName = "Navi.html";
        private bool _isPlugin = false;

        // für die Aktualisierung der Daten --> währenddessen kein Zugriff
        private static bool _isPreparing = true;

        private string[] _parameter;
        private string _response;

        private StreamWriter _sw;
        private Response _resp = new Response();

        private static Dictionary<string, List<string>> _StreetCity = new Dictionary<string, List<string>>();



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
            Parameter = parameter;
        }




        // ##########################################################################################################################################
        public void Run()
        {
            // Beim Programmstart wird das XML-File in einem eigenen Thread automatisch eingelesen
            if (_parameter == null)
            {
                // Status setzen, damit währenddessen nicht drauf zugegriffen werden kann
                _isPreparing = true;

                Thread thread = new Thread(Preparing);
                thread.Start();
            }

            // Falls das Plugin ohne Parameter aufgerufen wird
            else if (_parameter[0] == _pluginName)
            {
                DisplayForm();
            }

            // Plugin soll das OSM (XML) - File erneut einlesen und ins Dictionary speichern
            else if (_parameter[0] == "Preparing")
            {
                PrepareXMLFile();
            }

            // Es wird eine Inputform für den Straßennamen ausgegeben
            else if (_parameter[0] == "Navigation")
            {
                DisplayInputForm();
            }

            // Es wurde ein Straßennamen eingegeben und dieser soll im Dictionary gesucht werden
            else if (_parameter[0] == "street")
            {
                PrepareAnswer();
            }
            else
            {
                throw new WrongParameterException("Navi ");
            }


            if (_parameter != null)
            {
                _resp.ContentType = "text/html";
                _resp.SendMessage(_sw, _response);
            }
        }


        // ##########################################################################################################################################
        private void Preparing()
        {
            // Zeitmessung - BEGINN
            Console.Beep();

            DateTime date_begin = DateTime.Now;
            Console.WriteLine(date_begin.ToString());



            // Pfad am Server --> gesamter Inhalt wird ausgegeben
            string path = "C:\\Ress\\austria-latest.osm";
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


            // Zeitmessung - ENDE
            Console.Beep();

            DateTime date_end = DateTime.Now;
            Console.WriteLine(date_end.ToString());
            Console.WriteLine(date_end - date_begin);

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("DICTIONARY WURDE VOLLSTÄNDIG AKTUALISIERT");
            Console.WriteLine();
            Console.WriteLine();

            _response = "Ihr Datenquelle " + path + " wurde erfolgreich aktualisiert!";

            _isPreparing = false;
        }



        // ##########################################################################################################################################
        private void DisplayForm()
        {
            _response =
                @"
                    <button><a href='Navi.html?Preparing'>Stra&szlig;enkarte neu aufbereiten</a></button>
                    <br />
                    <button><a href='Navi.html?Navigation'>Stra&szlig;en <--> Orte</a></button>
                ";
            _resp.SendMessage(_sw, _response);

        }


        // ##########################################################################################################################################
        private void PrepareXMLFile()
        {
            if (_isPreparing == false)
            {
                _isPreparing = true;

                Thread thread = new Thread(Preparing);
                thread.Start();

                _response = "Bitte warten Sie, w&auml;hrend die Daten aktualisiert werden";
            }
            else
            {
                _response = "Die Daten werden gerade aktualisiert. Bitte probieren Sie es sp&auml;ter erneut!";
            }
        }


        // ##########################################################################################################################################
        private void DisplayInputForm()
        {
            _response += @"
                <form method='POST' action='Navi.html?NavigationSearch'>
                    <label>Strassenname</label>
                    <input type='text' name='street' value='Barrett Lane' />
                    </br>
                    <input type='submit' value='Submit' />
                </form>
            ";
        }


        // ##########################################################################################################################################
        private void PrepareAnswer()
        {
            bool exists = false;

            if (_isPreparing == false)
            {
                string decodedParameter = _parameter[1].Replace("+", " ");

                string decodedParam = HttpUtility.UrlDecode(decodedParameter);

                foreach (KeyValuePair<string, List<string>> kvp in _StreetCity)
                {
                    try
                    {
                        if (kvp.Key == decodedParam)
                        {
                            exists = true;

                            int size = kvp.Value.Count;

                            for (int i = 0; i < size; ++i)
                            {
                                _response += kvp.Value[i];
                                _response += "<br />";
                            }
                        }
                    }
                    catch
                    {
                        _response = "Kein Eintrag vorhanden";
                    }
                }

                if (exists == false)
                {
                    throw new WrongParameterException("Navi ");
                }

            }
            else
            {
                _response = "Die Daten werden gerade aktualisiert. Bitte probieren Sie es sp&auml;ter erneut!";
            }
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


        public string Param
        {
            get { return _parameter[0]; }
        }
    }
}
