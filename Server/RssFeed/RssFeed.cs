using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Web;
using System.Data.SqlClient;
using System.Data.OleDb;
using Server;

namespace Server
{
    public class RssFeed : IPlugins
    {        
        
        private string _pluginName = "RssFeed.html";
        private bool _isPlugin = false;
        private string[] _parameter;
        private string _response;

        private StreamWriter _sw;
        private Response _resp = new Response();

        private SqlConnection _db;


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

            // Database
            string[] dbData = new string[] {".\\SqlExpress", "SWE_Temperatur", "true"};
            
            _db = new SqlConnection(@"Data Source=" + dbData[0] + "; Initial Catalog=" + dbData[1] + "; Integrated Security=" + dbData[2] + ";");
        }


        // ##########################################################################################################################################
        public void Run()
        {
            if (_parameter[0] == _pluginName)
            {
                DisplayForm();
            }

            else if (_parameter[0] == "Show")
            {
                DisplayInputForm();
            }

            else if (_parameter[0] == "Edit")
            {
                DisplayEditForm();
            }

            else if (_parameter[0] == "EditSave")
            {
                EditSave();
            }

            else if (_parameter[0] == "EditDelete")
            {
                EditDelete();
            }

            else if (_parameter[0] == "Feed")
            {
                DisplayFeed();
            }
        }



        // ##########################################################################################################################################
        private void DisplayForm()
        {
            _response += @"
                <button><a href='RssFeed.html?Show'>Rss-Feed ausgeben</a></button>
                <br />
                <button><a href='RssFeed.html?Edit'>Rss-Feed bearbeiten</a></button>
            ";

            _resp.ContentType = "text/html";
            _resp.SendMessage(_sw, _response);

        }

        // ##########################################################################################################################################
        private void DisplayFeed()
        {                
            string path = null;
                
            using (_db)
            {
                _db.Open();

                // 
                SqlCommand cmd = new SqlCommand("SELECT [RSSFEED].[NAME], [RSSFEED].[FEED] FROM [RSSFEED]", _db);

                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        if (rd.GetString(0) == _parameter[1])
                        {
                            path = rd.GetString(1);
                        }
                    }
                }
            }

            XmlTextReader reader = new XmlTextReader(path);

            string type = null;
            string[] _rss = new string[4];

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        type = reader.Name;
                        break;

                    case XmlNodeType.Text:

                        if (type == "title")
                        {
                            _rss[0] = reader.Value;
                        }
                        else if (type == "link")
                        {
                            _rss[1] = reader.Value;
                        }
                        else if (type == "description")
                        {
                            _rss[2] = reader.Value;
                        }
                        else if (type == "pubDate")
                        {
                            _rss[3] = reader.Value;
                        }
                        break;

                    case XmlNodeType.EndElement:
                        if (reader.Name == "item")
                        {
                            _response += "<a href='" + _rss[1] + "'>" + _rss[0] + "</a>";
                            _response += "<br />";
                            _response += _rss[3];
                            _response += "<br />";
                            _response += _rss[2]; 
                            _response += "<br />";
                            _response += "<br />";
                        }
                        break;
                }
            }
            _resp.ContentType = "text/html";
            _resp.SendMessage(_sw, _response);
        }


        // ##########################################################################################################################################
        private void DisplayInputForm()
        {
            using (_db)
            {
                _db.Open();

                SqlCommand cmd = new SqlCommand("SELECT [RSSFEED].[NAME], [RSSFEED].[FEED] FROM [RSSFEED]", _db);

                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    _response += @"
                    <form method='POST' action='RssFeed.html?ShowFeed'>
                        <label>RSS-Feed-Link</label>
                    
                        <select name='Feed'>";

                    while (rd.Read())
                    {
                        _response += "<option>" + rd.GetString(0) + "</option>";
                    }

                    _response += @"
                        </select>
                        </br>
                        <input type='submit' value='Submit' />
                    </form>
                    ";
                }
            }

            _resp.ContentType = "text/html";
            _resp.SendMessage(_sw, _response);
        }


        // ##########################################################################################################################################
        private void DisplayEditForm()
        {
            // ADD
            _response += @"
                <div id='RssFeedSave' class='feedContainer'>
                    <h2>Add</h2>
                    <form method='POST' action='RssFeed.html?EditSave'>
                        <label>Feedname</label>
                        <input type='text' name='EditSave' value='' />
                        <label>URL</label>
                        <input type='text' name='FeedUrl' value='' size='100' />
                        </br>
                        <input type='submit' value='Add' />
                    </form>
                </div>
            ";



            // DELETE
            using (_db)
            {
                _db.Open();

                SqlCommand cmd = new SqlCommand("SELECT [RSSFEED].[NAME], [RSSFEED].[FEED] FROM [RSSFEED]", _db);

                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    _response += @"
                    <div id='RssFeedDelete' class='feedContainer'>
                        <h2>Delete</h2>
                        <form method='POST' action='RssFeed.html?EditDelete'>
                            <label>RSS-Feed-Link</label>
                    
                            <select name='EditDelete'>";

                        while (rd.Read())
                        {
                            _response += "<option>" + rd.GetString(0) + "</option>";
                        }

                        _response += @"
                            </select>
                            </br>
                            <input type='submit' value='Delete' />
                        </form>
                    </div>
                    ";
                }
            }

            _resp.ContentType = "text/html";
            _resp.SendMessage(_sw, _response);
        }


        // ##########################################################################################################################################
        private void EditSave()
        {
            using (_db)
            {
                _db.Open();

                SqlCommand insert = new SqlCommand(@"INSERT INTO [RSSFEED] ([NAME], [FEED], [TIMESTAMP]) VALUES ('" + _parameter[1] + "', '" + HttpUtility.UrlDecode(_parameter[3]) + "', getdate())", _db);
                insert.ExecuteNonQuery();

                _db.Close();
            }

            _response = "Eintrag wurde gespeichert";

            _resp.ContentType = "text/html";
            _resp.SendMessage(_sw, _response);
        }


        // ##########################################################################################################################################
        private void EditDelete()
        {
            using (_db)
            {
                _db.Open();

                SqlCommand delete = new SqlCommand(@"DELETE FROM [dbo].[RSSFEED] WHERE [NAME] = '" + _parameter[1] + "'", _db);
                delete.ExecuteNonQuery();

                _db.Close();
            }

            _response = "Eintrag wurde gel&ouml;scht";

            _resp.ContentType = "text/html";
            _resp.SendMessage(_sw, _response);
        }
    }
}
