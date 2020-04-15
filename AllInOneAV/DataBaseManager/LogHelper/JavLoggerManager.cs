using DataBaseManager.JavDataBaseHelper;
using Model.JavModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseManager.LogHelper
{
    public class JavLoggerManager
    {
        public static void WriteLog(JavLibraryLog log)
        {
            JavDataBaseManager.WriteLog(log);
            if (!string.IsNullOrEmpty(log.URL))
            {
                JavDataBaseManager.WriteSecondTry(log);
            }
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
