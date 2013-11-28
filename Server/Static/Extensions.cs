using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server;

namespace Server
{
    class Extensions
    {

        public string checkExtensions(string FileExtension)
        {
            switch (FileExtension)
            {

                // ##########################################################################################################################################
                // Images
 
                case "jpg":
                    return "image/jpeg";

                case "jpeg":
                    return "image/jpeg";

                case "jpe":
                    return "image/jpeg";

                case "png":
                    return "image/png";

                case "gif":
                    return "image/gif";

                case "ico":
                    return "image/x-icon";




                // ##########################################################################################################################################
                // Text

                case "txt":
                    return "text/html";

                case "htm":
                    return "text/html";

                case "html":
                    return "text/html";

                case "shtml":
                    return "text/html";


                case "xml":
                    return "text/xml";

                case "csv":
                    return "text/comma-separated-values";




                // ##########################################################################################################################################
                // Applications
                /*
                case "htm":                                      // HTM
                    return "application/xhtml+xml";

                case "html":                                     // HTML
                    return "application/xhtml+xml";

                case "shtml":                                     // SHTML
                    return "application/xhtml+xml";
                */
                case "xhtml":                                     // XHTML
                    return "application/xhtml+xml";


                case "php":                                       // PHP
                    return "application/x-httpd-php";

                case "phtml":                                     // PHP
                    return "application/x-httpd-php";


                case "tar":                                     // TAR
                    return "application/x-tar";

                case "zip":                                     // ZIP
                    return "application/zip";


                case "xls":                                     // Excel
                    return "application/msexcel";

                case "xla":                                     // Excel
                    return "application/msexcel";


                case "ppt":                                     // PowerPoint
                    return "application/mspowerpoint";

                case "ppz":                                     // PowerPoint
                    return "application/mspowerpoint";

                case "pps":                                     // PowerPoint
                    return "application/mspowerpoint";

                case "pot":                                     // PowerPoint
                    return "application/mspowerpoint";


                case "doc":                                     // Word
                    return "application/msword";

                case "dot":                                     // Word
                    return "application/msword";


                case "pdf":                                     // PDF-Dateien
                    return "application/pdf";



                // ##########################################################################################################################################
                // Video

                case "mpeg":                                     // MPEG-Dateien
                    return "video/mpeg";

                case "mpg":                                     // MPEG-Dateien
                    return "video/mpeg";

                case "mpe":                                     // MPEG-Dateien
                    return "video/mpeg";


                case "mov":                                     // Quicktime-Dateien
                    return "video/quicktime";


                case "avi":                                     // Microsoft AVI-Dateien
                    return "video/x-msvideo";


                case "movie":                                     // Movie-Dateien
                    return "video/x-sgi-movie";




                // ##########################################################################################################################################
                // Default
                
                default:
                    return "application/octet-stream";

            }

        }
    }
}
