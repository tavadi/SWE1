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
                var filename = "LogFile";
                var extension = ".txt";
                int filecount = 0;


                StreamWriter sw = File.AppendText(filename + extension);

                FileInfo fi = new FileInfo(filename + extension);


                foreach (string files in System.IO.Directory.GetFiles(Environment.CurrentDirectory, "*.txt"))
                {
                    filecount++;
                    //Console.WriteLine("FILES " + filecount);
                }

                if (fi.Length > 1048576)        // 1 MB = 1048576 Byte
                {
                    sw.Close();
                    File.Move(filename + extension, filename + "_" + filecount + extension);

                    sw = File.AppendText(filename+ extension);
                }

                string error = DateTime.Now + ": " + System.Environment.NewLine + exp + System.Environment.NewLine;
                sw.WriteLine(error);
                sw.Close();

            }
            catch (FileNotFoundException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: File not Found Exeption - LogFile.txt" + System.Environment.NewLine + e + System.Environment.NewLine);
                Console.ForegroundColor = ConsoleColor.Green;
            }
            catch (FileLoadException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: File cannot load - LogFile.txt" + System.Environment.NewLine + e + System.Environment.NewLine);
                Console.ForegroundColor = ConsoleColor.Green;
            }
            catch (IOException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: File currently unavailable - LogFile.txt" + System.Environment.NewLine + e + System.Environment.NewLine);
                Console.ForegroundColor = ConsoleColor.Green;
            }
        }
    }
}
