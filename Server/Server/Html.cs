using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Html
    {

        private string _htmlHeader;
        private string _htmlFooter;


        public Html()
        {
            CreateHtmlHeader();
            CreateHtmlFooter();
        }

        // ##########################################################################################################################################
        public string HtmlHeader
        {
            get
            {
                return _htmlHeader;
            }
        }


        // ##########################################################################################################################################
        public string HtmlFooter
        {
            get
            {
                return _htmlFooter;
            }
        }




        // ##########################################################################################################################################
        private void CreateHtmlHeader()
        {
            _htmlHeader =
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

                            .feedContainer {
                                margin-top:5%;
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


                                for (var i = 1; i <= groups; ++i)
                                {
                                    $('#navigation').append('<a href=""#"" id=' + i + ' onClick=""javascript:show_data(this.id)"">' + ' ' + i + ' ' + '</a>');
                                }

                                /*
                                for (var i = 1; i <= groups; ++i)
                                {
                                    $( '#grouplink' + i ).click(function() {

                                        $( '#group' + i ).css( 'visibility', 'visible' );
                                    });
                                }
                                */
                            }      

                            function show_data(id)
                            {
                                for (var i = 1; i <= groups; ++i)
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
        private void CreateHtmlFooter()
        {
            _htmlFooter += @"
                    </body> 
                </html>";
        }
    }
}
