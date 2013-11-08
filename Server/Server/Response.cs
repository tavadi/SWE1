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

        private string _Response;
        private string _ContentType;

        private string _HTMLHeader;
        private string _HTMLFooter;

        private int _size;

        private StreamWriter _sw;

        
        // ##########################################################################################################################################
        public void sendMessage(StreamWriter sw, string Response, string ContentType)
        {
            _sw = sw;
            _Response = Response;
            _ContentType = ContentType;

            // Wenn XML --> nur die reinen XML-Daten zurückschicken
            if (_ContentType == "text/html")
            {
                HTMLHeader();
                HTMLFooter();

                _size = _HTMLHeader.Length + _HTMLFooter.Length + _Response.Length;

                // Response abschicken
                SendHTTPHeader();
                SendHTMLHeader();
                _sw.WriteLine(_Response);
                SendHTMLFooter();
            }

            // Wenn es "text/html" ist, die Nachrichten in die HTML-Seite einbetten
            else if (_ContentType == "text/xml")
            {
                _size = _Response.Length;

                SendHTTPHeader();
                _sw.WriteLine(_Response);
            }




            // Inhalt löschen
            Response = "";



            _sw.Flush();
        }



        // ##########################################################################################################################################
        public void sendMessage(StreamWriter sw, byte[] Message, string ContentType)
        {
            _sw = sw;
            _size = Message.Length;
            _ContentType = ContentType;

            SendHTTPHeader();
            _sw.BaseStream.Write(Message, 0, Message.Length);
            _sw.Flush();
        }


        // ##########################################################################################################################################
        private void HTMLHeader()
        {
            _HTMLHeader =
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
            ";
        }



        // ##########################################################################################################################################
        private void HTMLFooter()
        {
            _HTMLFooter += @"
                    </body> 
                </html>";
        }








        // ##########################################################################################################################################
        private void SendHTTPHeader()
        {
            /*
            // HTTP-Header mit entsprechender Nachricht abschicken
            HTTPHeader Header = new HTTPHeader(_sw, _Response, _ContentType, _size);
            Header.sendMessage();
             * */


            _sw.WriteLine("HTTP/1.1 200 OK");
            _sw.WriteLine("Server: Apache/1.3.29 (Unix) PHP/4.3.4");
            _sw.WriteLine("Content-Length: " + _size);
            _sw.WriteLine("Content-Language: de");
            _sw.WriteLine("Connection: close");
            _sw.WriteLine("Content-Type: " + _ContentType);
            _sw.WriteLine();


        }


        // ##########################################################################################################################################
        private void SendHTMLHeader()
        {
            _sw.WriteLine(_HTMLHeader);
        }


        // ##########################################################################################################################################
        private void SendHTMLFooter()
        {
            _sw.WriteLine(_HTMLFooter);
        }
    }
}
