using DataBaseManager.JavDataBaseHelper;
using DataBaseManager.ScanDataBaseHelper;
using Model.JavModels;
using Model.ScanModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utils;

namespace WebService
{
    public class WebService
    {
        private static List<string> formats = JavINIClass.IniReadValue("Scan", "Format").Split(',').ToList();
        private static List<string> excludes = JavINIClass.IniReadValue("Scan", "Exclude").Split(',').ToList();

        public static List<AV> GetClosetMatch(string inputName)
        {
            List<AV> ret = new List<AV>();
            List<string> allPossibleMatch = new List<string>();
            inputName = inputName.ToUpper();

            if (!CacheTools.HasCache("cache-prefix"))
            {
                UpdateCache();
            }

            var allPrefix = CacheTools.GetCache<List<string>>("cache-prefix");

            foreach (var prefix in allPrefix)
            {
                if (inputName.Contains(prefix))
                {
                    var pattern = prefix + "{1}-?\\d{1,5}";
                    var possibleId = Regex.Match(inputName, pattern).Value;

                    if (possibleId.Contains("-"))
                    {
                        allPossibleMatch.Add(possibleId);
                    }
                    else
                    {
                        bool isFirst = true;
                        StringBuilder sb = new StringBuilder();

                        foreach (var c in possibleId)
                        {
                            if (c >= '0' && c <= '9')
                            {
                                if (isFirst)
                                {
                                    sb.Append("-");
                                    isFirst = false;
                                }
                            }
                            sb.Append(c);
                        }

                        allPossibleMatch.Add(sb.ToString());
                    }
                }
            }

            ret = JavDataBaseManager.GetAllAV(allPossibleMatch);
            ret = ret.OrderByDescending(x => x.AvLength).ToList();

            return ret;
        }

        public static List<AV> GetAv(string orderStr, string where, string pageStr)
        {
            return JavDataBaseManager.GetAllAVOnWhere(orderStr, where, pageStr);
        }

        public static List<CommonModel> GetNames(string type)
        {
            List<CommonModel> ret = new List<CommonModel>();
            JavDataBaseManager.GetCommonMode(type).ForEach(x => ret.Add(new CommonModel { Name = x.Name, Type = type, Url = "http://www.cainqs.com:8087/avapi/getav?" + type + "=" + x.Name }));

            return ret;
        }

        public static List<MatchMap> GetMatch(string orderStr, string where, string pageStr)
        {
            return ScanDataBaseManager.GetMatchMap(orderStr, where, pageStr);
        }

        public static List<UnmatchVW> GetUnMatch(bool includePlayed)
        {
            List<UnmatchVW> ret = new List<UnmatchVW>();

            var drivers = Environment.GetLogicalDrives();
            var folder = "pt";

            foreach (var driver in drivers)
            {
                if (Directory.Exists(driver + "\\" + folder))
                {
                    List<FileInfo> fi = new List<FileInfo>();
                    var files = FileUtility.GetFilesRecursive(driver + "\\" + folder, formats, excludes, fi, 500);

                    foreach (var f in fi)
                    {
                        UnmatchVW temp = new UnmatchVW();
                        temp.FileName = f.Name.Replace(f.Extension, "");
                        temp.FilePath = f.FullName;
                        temp.FileSize = FileSize.GetAutoSizeString(f.Length, 2);
                        temp.FileExts = f.Extension;

                        if (ScanDataBaseManager.ViewedFile(FileUtility.ReplaceInvalidChar(f.FullName)))
                        {
                            temp.HasPlayed = true;
                        }

                        if (includePlayed)
                        {
                            ret.Add(temp);
                        }
                        else
                        {
                            if (temp.HasPlayed == false)
                            {
                                ret.Add(temp);
                            }
                        }
                    }
                }
            }
            return ret;
        }

        private static void UpdateCache()
        {
            if (!CacheTools.HasCache("cache-prefix"))
            {
                var allPrefix = ScanDataBaseManager.GetPrefix().Select(x => x.Prefix).ToList();
                allPrefix = allPrefix.OrderByDescending(x => x.Length).ToList();

                CacheTools.CacheInsert("cache-prefix", allPrefix, DateTime.Now.AddDays(1));
            }
        }      
    }
}
