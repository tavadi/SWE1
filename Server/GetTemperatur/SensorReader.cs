using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;
using Server;

namespace Server
{
    class SensorReader
    {

        // ##########################################################################################################################################
        public void InsertData()
        {
            // Timer der automatisch alle 10 Sekunden die Funktion "Insert" ausführt
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Elapsed += new ElapsedEventHandler(Insert);
            timer.Interval = 10000;
            timer.Enabled = true;

        }




        // ##########################################################################################################################################
        private void Insert(object source, ElapsedEventArgs e)
        {
            // Zufallszahlen zwischen 100 - 300
            Random rnd = new Random();
            float number = rnd.Next(100, 300);

            // durch 10 dividieren, wegen den Kommazahlen
            float Temp = (number / 10);

            // Kontrollausgabe
            Console.WriteLine("{0:0.00}", Temp);


            using (SqlConnection db = new SqlConnection(@"Data Source=.\SqlExpress;Initial Catalog=SWE_Temperatur; Integrated Security=true;"))
            {
                db.Open();

                // Komma durch Punkt ersetzen, weil der Wert in der DB als x.xx gespeichert werden muss
                string str = Convert.ToString(Temp);
                str = str.Replace(",", ".");

                SqlCommand insert = new SqlCommand(@"INSERT INTO [MESSDATEN] ([DATE], [TEMPERATUR], [TIMESTAMP]) VALUES (getdate(), " + str + ", getdate())", db);
                insert.ExecuteNonQuery();

                db.Close();

            }
        }
    }
}
