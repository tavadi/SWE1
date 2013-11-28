using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SqlClient;
using System.Data.OleDb;

namespace Server
{
    class createXML
    {

        // ##########################################################################################################################################
        public string Create(uint Year, uint Month, uint Day)
        {
            string xml;

            // XML erstellen
            xml =  @"<?xml version='1.0' encoding='UTF-8' standalone='yes'?>
                        <PluginTemperatur>
                            <title>Plugin Temperatur</title>
                    ";
    


            using (SqlConnection db = new SqlConnection(
                @"Data Source=.\SqlExpress;
                Initial Catalog=SWE_Temperatur;
	            Integrated Security=true;"))
            {
                db.Open();
                SqlCommand cmd = new SqlCommand(@"SELECT [DATE], [TEMPERATUR] 
                                            FROM [MESSDATEN] 
                                            WHERE YEAR([DATE]) = '" + Year + "' AND MONTH([DATE]) = '" + Month + "' AND DAY([DATE]) = '" + Day + "'", db);

                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        xml += @"
                                <element>
                                    <Date>" + Year + "." + Month + "." + Day + @"</Date>
                                    <Temperature>" + rd.GetDecimal(1).ToString() + @"</Temperature>
                                </element>"
;

                    }
                }
            }

            xml +=      "</PluginTemperatur>";


            return xml;
        }

    }
}
