using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;


namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {

            TcpClient client = new TcpClient();
            client.Connect("localhost", 8080);

            Console.WriteLine("Connectiong ...");

            NetworkStream stream = client.GetStream();
            StreamWriter sw = new StreamWriter(stream);

            string input = "";

            while (input != "exit")
            {
                input = Console.ReadLine();

                sw.WriteLine("GET / HTTP/1.1");
                sw.WriteLine("host: localhost");
                sw.WriteLine("connection: close");
                sw.WriteLine(input);
                sw.WriteLine();
                sw.Flush();
            }


            Console.WriteLine("Connection close ...");
            Console.ReadLine();


        }
    }
}
