using MangaDownloaderGUI.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangaDownloaderGUI.Service
{
    public class LogService
    {
        private static readonly string ErrorFile = Environment.CurrentDirectory + "\\error.json";

        public static void WriteLog(string function, string exception)
        {
            if (!File.Exists(ErrorFile))
            {
                File.Create(ErrorFile).Close();
            }

            ErrorEntity error = new ErrorEntity
            {
                Function = function,
                Exception = exception
            };

            StreamWriter sw = new StreamWriter(ErrorFile);
            sw.WriteLine(JsonConvert.SerializeObject(error));
            sw.Close();
        }
    }
}
