using DataBaseManager.JavDataBaseHelper;
using Model.Common;
using Model.JavModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateReport
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, AV> match = new Dictionary<string, AV>();
            ReportModel report = new ReportModel();
            report.Top = 10;

            Console.WriteLine("正在获取AV爬虫信息...");
            var filesContainer = ReportHelper.GenerateExistingAVs();
            Console.WriteLine("正在获取本地AV信息...");
            var avs = JavDataBaseManager.GetAllAV();

            Console.WriteLine("正在更新本地Mapping信息...");
            var mapCount = ReportHelper.UpdateScanMap(filesContainer, avs);
            Console.WriteLine("更新了" + mapCount + "条...");

            Console.WriteLine("正在生成基本报表...");
            ReportHelper.GenerateBasicReport(filesContainer, avs, match, report);
            Console.WriteLine("正在生成匹配报表...");
            ReportHelper.GenerateMatchReport(filesContainer, avs, match, report);
            Console.WriteLine("正在生成报表其他部分...");
            ReportHelper.GenerateOtherReport(filesContainer, avs, match, report);
            Console.WriteLine(ReportHelper.GenerateActuralReport(report));

            //ReportHelper.Test(filesContainer, avs, match, report);

            Console.ReadKey();
        }
    }
}
