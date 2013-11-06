using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Web;

namespace Server
{
    public class Response
    {
        private LogFile LogFile;

        private string _message;

        
        // ##########################################################################################################################################
        public IList<string> Message
        {
            set
            {
                try
                {
                    foreach (string i in value)
                    {
                        _message = _message + i + "<br />";
                    }
                }
                catch (NullReferenceException e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Es wurden keine Werte übergeben");
                    Console.ForegroundColor = ConsoleColor.Green;

                    LogFile = new LogFile(e.ToString());
                }
            }
        }


        
        // ##########################################################################################################################################
        public void sendMessage(StreamWriter sw)
        {

            string msg =
                @"
                <html>
                    <head> 
                        <title>SensorCloud</title> 

                        <script src='//ajax.googleapis.com/ajax/libs/jquery/1.10.2/jquery.min.js'></script>

                        <style type=""text/css"">
                            
                            #container {
                                width:100%;
                            }
                            
                            .line {
                                padding:10px;
                                width:100%;
                            }

                            .min20 {
                                width:20%;
                                float:left;
                                background-color:#D8D8D8;
                            }

                            .group {
                                visibility:hidden;
                                width:100%;
                                position:absolute;
                                padding-left:20%;
                            }



                        </style>




                        <script language='javascript' type='text/javascript'>

                            var groups = 1;

                            function countGroups()
                            {
                                groups = $('.group').length

                                //alert(groups);


                                // Ersten Ergebnisse sichtbar machen
                                $( '#group' + 1 ).css( 'visibility', 'visible' );


                                for (var i = 1; i <= groups; i++)
                                {
                                    $('#navigation').append('<a href=""#"" id=""' + i + '"" onClick=""javascript:show_data(this.id)"">' + ' ' + i + ' ' + '</a>');
                                }

                                /*
                                for (var i = 1; i <= groups; i++)
                                {
                                    $( '#grouplink' + i ).click(function() {

                                        $( '#group' + i ).css( 'visibility', 'visible' );
                                    });
                                }
                                */
                            }      

                            function show_data(id)
                            {
                                for (var i = 1; i <= groups; i++)
                                {
                                    $( '#group' + i ).css( 'visibility', 'hidden' );
                                }

                                $( '#group' + id ).css( 'visibility', 'visible' );
                            }                     

                        </script>

                    </head> 
                    <body>
                        <script>
                        window.onload=countGroups;
                        </script>

                        <div id='navigation'></div>

                        "
                        + _message +
                        @"
                    </body> 
                </html>";

            HTTPHeader Header = new HTTPHeader(sw, msg);
            Header.sendMessage();

            // Inhalt löschen
            _message = "";
        }

    }
}
