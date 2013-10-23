using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Server
{
    class LogFile
    {
        public LogFile(string exp)
        {

            try
            {
                StreamWriter sw = File.AppendText("LogFile.txt");
                string error = DateTime.Now + ": " + exp + System.Environment.NewLine;
                sw.WriteLine(error);
                sw.Close();
            }
            catch (FileNotFoundException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: File not Found Exeption - LogFile.txt" + System.Environment.NewLine + e);
                Console.ForegroundColor = ConsoleColor.Green;
            }
            catch (FileLoadException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: File cannot load - LogFile.txt" + System.Environment.NewLine + e);
                Console.ForegroundColor = ConsoleColor.Green;
            }
            catch (IOException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: File currently unavailable - LogFile.txt" + System.Environment.NewLine + e);
                Console.ForegroundColor = ConsoleColor.Green;
            }
        }
    }
}
