using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class LogHelper
    {
        public static string root = "C:/AvLog/";

        public static void WriteLog(string logFile, string content)
        {
            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }

            if (!File.Exists(root + logFile))
            {
                File.Create(root + logFile).Close();
            }

            StreamWriter sw = new StreamWriter(root + logFile, true, Encoding.UTF8);
            sw.WriteLine(content);
            sw.Close();
        }
    }
}
