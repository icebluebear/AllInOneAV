using DataBaseManager.JavDataBaseHelper;
using DataBaseManager.ScanDataBaseHelper;
using Model.ScanModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Utils;
using Newtonsoft.Json;

namespace ScanAllAndMatch
{
    public class Process
    {
        private static StringBuilder sb = new StringBuilder();
        private static List<string> formats = JavINIClass.IniReadValue("Scan", "Format").Split(',').ToList();
        private static List<string> excludes = JavINIClass.IniReadValue("Scan", "Exclude").Split(',').ToList();
        private static List<string> folders = JavINIClass.IniReadValue("Scan", "Folder").Split(',').ToList();

        public static void Start()
        {
            try
            {
                sb.AppendLine(string.Format("开始扫描 {0}", DateTime.Now.ToLongTimeString()));
                //var drivers = Environment.GetLogicalDrives().Skip(1).ToList();
                List<FileInfo> fi = new List<FileInfo>();
                List<Match> temp = new List<Match>();

                foreach (var driver in folders)
                {
                    sb.AppendLine(string.Format("添加扫描驱动器: {0}", driver));
                    Console.WriteLine("Processing " + driver);
                    if (!string.IsNullOrEmpty(FileUtility.GetFilesRecursive(driver, formats, excludes, fi, 100)))
                    {
                        sb.AppendLine("获取文件失败");
                    }
                }

                var avs = JavDataBaseManager.GetAllAV();
                var prefix = FileUtility.GetPrefix(avs);

                Console.WriteLine("Fi -> " + fi.Count);
                Console.WriteLine("AV -> " + avs.Count + "   Prefix -> " + prefix.Count);

                sb.AppendLine(string.Format("符合条件文件: {0}, 总共AV: {1}", fi.Count, avs.Count));

                foreach (var file in fi)
                {
                    var scan = new Scan
                    {
                        FileName = file.Name.Trim().ToUpper(),
                        Location = file.DirectoryName.Trim().ToLower(),
                        Size = FileSize.GetAutoSizeString(file.Length, 2)
                    };

                    var possibleIDs = FileUtility.GetPossibleID(scan, prefix);

                    possibleIDs = possibleIDs.OrderByDescending(x => x.Length).Take(1).ToList();

                    sb.AppendLine(string.Format("文件{0}可能的Match有{1}", file, possibleIDs.Count));

                    Console.WriteLine("PossibleIDs -> " + possibleIDs.Count);

                    AddTemp(scan, possibleIDs, temp);
                }

                Console.WriteLine("Temp -> " + temp.Count);

                sb.AppendLine(string.Format("一共找到{0}个Match", temp.Count));

                ScanDataBaseManager.ClearMatch();

                foreach (var m in temp)
                {
                    Console.WriteLine(string.Format("Insert {0}\\{1}", m.Location, m.Name));
                    sb.AppendLine(string.Format("在库中添加Match -> {0}", m.Location));
                    ScanDataBaseManager.SaveMatch(m);
                }

                sb.AppendLine("更新数据库状态");
                ScanDataBaseManager.InsertFinish();

                var duplicateItemList = new List<DuplicateItem>();
                
                var tempDic = temp.GroupBy(x => x.AvID.ToLower()).ToDictionary(x => x.Key, y => y.ToList());
                foreach (var item in tempDic)
                {
                    var tempItem = new DuplicateItem();
                    tempItem.AvId = item.Key;
                    tempItem.Matches = item.Value;

                    duplicateItemList.Add(tempItem);
                }

                var jsonRoot = "C:/AvLog/";
                var jsonStr = JsonConvert.SerializeObject(duplicateItemList).Replace("\\","\\\\");
                var jsonFile = "ScanJson" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") + ".json";
                LogHelper.WriteLog(jsonFile, jsonStr);
                EmailHelper.SendEmail("ScanJson", "详情见附件", new[] { "cainqs@outlook.com" }, new[] { jsonRoot + jsonFile });
            }
            catch (Exception e)
            {
                sb.AppendLine(e.ToString());
            }
            finally
            {
                sb.AppendLine(string.Format("扫描结束 {0}", DateTime.Now.ToLongTimeString()));

                //var root = "C:/AvLog/";
                //var file = "ScanAndMatch" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") + "-log.txt";
                //LogHelper.WriteLog(file, sb.ToString());
                //EmailHelper.SendEmail("ScanAndMatchLog", "详情见附件", new[] { "cainqs@outlook.com" }, new[] { root + file });
            }
        }

        private static void AddTemp(Scan scan, List<string> possibleIDs, List<Match> temp)
        {
            foreach (var id in possibleIDs)
            {
                var avs = JavDataBaseManager.GetAllAV(id);

                foreach (var av in avs)
                {
                    temp.Add(new Match
                    {
                        AvID = av.ID.ToLower(),
                        AvName = av.Name.ToLower(),
                        Location = scan.Location.ToLower(),
                        Name = scan.FileName.ToLower()
                    });

                    sb.AppendLine(string.Format("文件{0}找到一个符合条件的AV -> {1}", scan.FileName, av.ID));
                }
            }
        }
    }
}
