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

        private decimal _id;
        private string _name;
        private string _url;

        private StreamWriter _sw;
        private Response _resp = new Response();

        private DBHandler _dbHandler = new DBHandler();
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

            _db = _dbHandler.Connection;
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

            else if (_parameter[0] == "EditUpdate")
            {
                EditUpdate();
            }

            else if (_parameter[0] == "EditDelete")
            {
                EditDelete();
            }

            else if (_parameter[0] == "Feed")
            {
                string path = PrepareUrl();
                DisplayFeed(path);
            }

            else
            {
                throw new WrongParameterException("RssFeed");
            }


            _resp.ContentType = "text/html";
            _resp.SendMessage(_sw, _response);
        }


        // ##########################################################################################################################################
        public void DisplayForm()
        {
            _response += @"
                <button><a href='RssFeed.html?Show'>Rss-Feed ausgeben</a></button>
                <br />
                <button><a href='RssFeed.html?Edit'>Rss-Feed bearbeiten</a></button>
            ";
        }


        // ##########################################################################################################################################
        private string PrepareUrl()
        {                
            string path = null;

            // URL zum passenden FeedNamen herausfiltern
            using (_db)
            {
                _db.Open();

                SqlCommand cmd = new SqlCommand("SELECT [RSSFEED].[NAME], [RSSFEED].[FEED] FROM [RSSFEED] WHERE [RSSFEED].[NAME] = '" + _parameter[1] + "'", _db);

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

            return path;
        }


        // ##########################################################################################################################################
        private void DisplayFeed(string path)
        {
            XmlTextReader reader = new XmlTextReader(path);

            string type = null;
            string[] _rss = new string[4];

            try
            {
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
            }
            catch (FileNotFoundException e)
            {
                throw new WrongFilenameException("RssFeed", e);
            }
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
                        <input type='submit' value='Anzeigen' />
                    </form>
                    ";
                }
            }
        }


        // ##########################################################################################################################################
        private void DisplayEditForm()
        {
            // ADD
            _response += @"
                <div id='RssFeedSave' class='feedContainer'>
                    <h2>Add</h2>
                    <form method='POST' action='RssFeed.html?EditSave'>
                        <input type='text' name='EditSave' value='0' style='display:none;'/>
                        <label>Feedname</label>
                        <input type='text' name='FeedName' value='' size='50' />
                        <label>URL</label>
                        <input type='text' name='FeedUrl' value='' size='100' />
                        </br>
                        <input type='submit' value='Add' />
                    </form>
                </div>
            ";



            using (_db)
            {
                _db.Open();

                // DELETE
                SqlCommand cmd = new SqlCommand("SELECT [RSSFEED].[ID], [RSSFEED].[NAME] FROM [RSSFEED]", _db);

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
                            _response += "<option>" + rd.GetString(1) + "</option>";
                        }

                        _response += @"
                            </select>
                            </br>
                            <input type='submit' value='Delete' />
                        </form>
                    </div>
                    ";
                }


                // UPDATE
                cmd = new SqlCommand("SELECT [RSSFEED].[ID], [RSSFEED].[NAME], [RSSFEED].[FEED] FROM [RSSFEED]", _db);

                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    _response += @"
                        <div id='RssFeedUpdate' class='feedContainer'>
                            <h2>Update</h2>";

                    while (rd.Read())
                    {                      
                         _response += @"
                             <form method='POST' action='RssFeed.html?EditUpdate'>
                                 <label>RSS-Feed-Link</label>
                                 <input type='text' name='EditUpdate' value='" + rd.GetDecimal(0).ToString() + @"' />
                                 <input type='text' name='FeedName' value='" + rd.GetString(1) + @"' size='50' />
                                 <input type='text' name='FeedUrl' value='" + rd.GetString(2) + @"' size='100' />
                                 <input type='submit' value='Save' />
                             </form>";
                         
                    }
                    _response += @"
                                </br>
                        </div>
                        ";
                }
            }
        }


        // ##########################################################################################################################################
        private void EditSave()
        {
            ParseParameters();

            using (_db)
            {
                _db.Open();

                SqlCommand insert = new SqlCommand(@"INSERT INTO [RSSFEED] ([NAME], [FEED], [TIMESTAMP]) VALUES ('" + _name + "', '" + HttpUtility.UrlDecode(_url) + "', getdate())", _db);
                insert.ExecuteNonQuery();

                _db.Close();
            }

            _response = "Eintrag wurde gespeichert";
        }


        // ##########################################################################################################################################
        private void EditUpdate()
        {
            ParseParameters();

            
            using (_db)
            {
                _db.Open();

                SqlCommand insert = new SqlCommand( @"UPDATE [RSSFEED] 
                                                        SET [NAME] = '" + _name + "', [FEED] = '" + HttpUtility.UrlDecode(_url) + @"', [TIMESTAMP] = getdate()
                                                        WHERE [ID] = " + _id + "", _db);
                insert.ExecuteNonQuery();

                _db.Close();
            }

            _response = "Eintrag wurde ge&auml;ndert";
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

                    if ((_parameter[i] == "EditSave") || (_parameter[i] == "EditUpdate"))
                    {
                        _id = Convert.ToDecimal(_parameter[a + 1]);
                    }

                    else if (_parameter[i] == "FeedName")
                    {
                        _name = _parameter[a + 1];
                    }

                    else if (_parameter[i] == "FeedUrl")
                    {
                        _url = _parameter[a + 1];
                    }
                }
            }
            catch (FormatException e)
            {
                throw new WrongParameterException("RssFeed ", e);
            }

            if ((_name == "") || (_url == ""))
            {
                throw new WrongParameterException("RssFeed: ");
            }
        }



        // ##########################################################################################################################################
        public decimal ID
        {
            get { return _id; }
        }

        // ##########################################################################################################################################
        public string Name
        {
            get { return _name; }
        }

        // ##########################################################################################################################################
        public string Url
        {
            get { return _url; }
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
