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
    class XmlCreator
    {

        // ##########################################################################################################################################
        public string Create(uint year, uint month, uint day)
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
                SqlCommand cmd = new SqlCommand(@"  SELECT [MESSDATEN].[DATE], [MESSDATEN].[TEMPERATUR] 
                                                    FROM [MESSDATEN] 
                                                    WHERE YEAR([DATE]) = '" + year + "' AND MONTH([DATE]) = '" + month + "' AND DAY([DATE]) = '" + day + "'", db);

                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        xml += @"
                                <element>
                                    <Date>" + year + "." + month + "." + day + @"</Date>
                                    <Temperature>" + rd.GetDecimal(1).ToString() + @"</Temperature>
                                </element>
                                ";
                    }
                }
            }

            xml +=      "</PluginTemperatur>";


            return xml;
        }

    }
}
