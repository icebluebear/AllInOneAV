using DataBaseManager.JavDataBaseHelper;
using Model.Common;
using Model.JavModels;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Utils;

namespace BatchJavScanerAndMacthMagUrl
{
    class Program
    {
        public static void Main(string[] args)
        {
            LockModel lockModel = new LockModel();

            if (args[0] == "daily")
            {
                JavLibraryHelper.DoDailyUpdate(true);
            }

            if (args[0] == "full")
            {
                JavLibraryHelper.DoFullScanSingleThread(true);
            }

            if (args[0] == "certain")
            {
                var titleArry = args[1].Split(',');
                var urlArray = args[2].Split(',');

                Dictionary<string, string> dic = new Dictionary<string, string>();

                for (int i = 0; i< titleArry.Length; i++)
                {
                    dic.Add(urlArray[i], titleArry[i]);
                }

                JavLibraryHelper.DoCertainCategory(dic, true);
            }
        }
    }
}
