using JavLibraryDownloader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavLibraryDownloader.LogHelper
{
    public class LoggerManager
    {
        public static void WriteLog(JavLibraryLog log)
        {
            DataBaseHelper.DataBaseManager.WriteLog(log);
        }

        public static JavLibraryLog GetInitLogger()
        {
            return new JavLibraryLog
            {
                Logger = "Init"
            };
        }

        public static JavLibraryLog GetScanLogger()
        {
            return new JavLibraryLog
            {
                Logger = "Scan"
            };
        }

        public static JavLibraryLog GetDownloadLogger()
        {
            return new JavLibraryLog
            {
                Logger = "Download"
            };
        }
    }
}
