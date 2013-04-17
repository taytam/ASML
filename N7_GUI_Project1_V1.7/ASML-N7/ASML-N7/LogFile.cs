using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASML_N7
{
    internal class LogFile
    {
        private StreamWriter sw;

        public LogFile()
        {
            
        }

        public void TakeinTargetFiles(List<Target> targetList)
        {
            if (!File.Exists("logfile.txt"))
            {
                sw = new StreamWriter("logfile.txt");
            }
            else
            {
                sw = File.AppendText("logfile.txt");
            }

            // Write to the file:
            sw.WriteLine(DateTime.Now);

            foreach (Target targ in targetList)
            {
                sw.WriteLine("Name= " + targ.Name);
                sw.WriteLine("\tXPos= " + targ.XPosition);
                sw.WriteLine("\tyPos= " + targ.YPosition);
                sw.WriteLine("\tzPos= " + targ.ZPosition);
                sw.WriteLine("\tisFriend= " + targ.isFriend);
                sw.WriteLine();
            }
            
            // Close the stream:
            sw.Close();
        }
    }
}