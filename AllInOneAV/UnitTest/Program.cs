using DataBaseManager.FindDataBaseHelper;
using DataBaseManager.JavDataBaseHelper;
using DataBaseManager.ScanDataBaseHelper;
using DataBaseManager.SisDataBaseHelper;
using HtmlAgilityPack;
using JavLibraryDownloader.InitHelper;
using Microsoft.Win32;
using Model.Common;
using Model.FindModels;
using Model.JavModels;
using Model.ScanModels;
using Model.SisModels;
using Newtonsoft.Json;
using Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.AccessControl;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ulog;
using Utils;

namespace UnitTest
{
    public class Program
    {
        #region SisDownloadParam
        private static string AsiaUncensoredAuthorshipSeed = JavINIClass.IniReadValue("Sis", "AsiaUncensoredAuthorshipSeed");
        private static string AsiaUncensoredSection = JavINIClass.IniReadValue("Sis", "AsiaUncensoredSection");
        private static string WesternUncensoredAuthorshipSeed = JavINIClass.IniReadValue("Sis", "WesternUncensoredAuthorshipSeed");
        private static string WesternUncensored = JavINIClass.IniReadValue("Sis", "WesternUncensored");
        private static string AsiaCensoredAuthorshipSeed = JavINIClass.IniReadValue("Sis", "AsiaCensoredAuthorshipSeed");
        private static string AsiaCensoredSection = JavINIClass.IniReadValue("Sis", "AsiaCensoredSection");
        private static string RootFolder = JavINIClass.IniReadValue("Sis", "root");
        private static string ListPattern = JavINIClass.IniReadValue("Sis", "ListPattern");
        private static string ListDatePattern = JavINIClass.IniReadValue("Sis", "ListDatePattern");
        private static string Prefix = JavINIClass.IniReadValue("Sis", "Prefix");
        private static readonly Dictionary<string, string> ChannelMapping = new Dictionary<string, string> { { AsiaCensoredAuthorshipSeed, "亚洲有码原创" }, { AsiaCensoredSection, "亚洲有码转帖" }, { WesternUncensoredAuthorshipSeed, "欧美无码原创" }, { WesternUncensored, "欧美无码转帖" }, { AsiaUncensoredAuthorshipSeed, "亚洲无码原创" }, { AsiaUncensoredSection, "亚洲无码转帖" } };
        #endregion

        private static LockModel lockModel = new LockModel();

        static void Main(string[] args)
        {
            RestoreCorrptedFile();

            Console.ReadKey();
        }

        private static void RestoreCorrptedFile()
        {
            Dictionary<string, List<SeedMagnetSearchModel>> seeds = new Dictionary<string, List<SeedMagnetSearchModel>>();
            List<string> fileList = new List<string>();
            var file = new DirectoryInfo("K:\\Fin").GetFiles();

            Parallel.ForEach(file, new ParallelOptions { MaxDegreeOfParallelism = 20 }, f =>
            {
                Console.WriteLine("处理 " + f.Name);
                 var split = f.Name.Split('-');
                 if (split.Length >= 3)
                 {
                     var searchContent = split[0] + "-" + split[1];

                     seeds.Add(searchContent, SearchSeedHelper.SearchSukebei(searchContent));
                 }
            });

            StringBuilder sb = new StringBuilder();

            foreach (var seed in seeds)
            {
                if (seed.Value != null && seed.Value.Count > 0)
                {
                    foreach (var model in seed.Value)
                    {
                        sb.AppendLine(model.MagUrl);
                    }
                }
                else
                {
                    Console.WriteLine(seed.Key + " 没找到磁链接");
                }
            }

            Console.WriteLine(sb.ToString());
        }

        private static void CleanrImg(string path)
        {
            var files = new DirectoryInfo(path).GetFiles();
        }

        private static void RestoreJavFromJson(string jsonFile)
        {
            if (File.Exists(jsonFile))
            {
                StreamReader sr = new StreamReader(jsonFile);
                var json = sr.ReadToEnd();
                sr.Close();

                List<AV> data = JsonConvert.DeserializeObject<List<AV>>(json);

                foreach (var av in data)
                {
                    JavDataBaseManager.InsertAV(av);
                }
            }
        }

        private static void ClearJav()
        {
            var all = JavDataBaseManager.GetAllAV();


            var idDIc = all.GroupBy(x => x.ID).ToDictionary(x => x.Key, x => x.ToList()).Where(x=>x.Value.Count > 1).ToDictionary(x => x.Key, x => x);
            var urlDic = all.GroupBy(x => x.URL).ToDictionary(x => x.Key, x => x.ToList()).Where(x => x.Value.Count > 1).ToDictionary(x => x.Key, x => x);

            var fileName = "G:\\JavAvBak.json";
            File.Create(fileName).Close();

            StreamWriter sw = new StreamWriter(fileName);

            sw.WriteLine(JsonConvert.SerializeObject(all));

            sw.Close();
        }

        private static void TestJavService()
        {
            JavLibraryHelper.DoDownloadOnly();
        }

        private static void TestBtsow()
        {
            var ret = BtsowClubHelper.SearchBtsow("vdd-123");
        }

        private static void GetAllRedundant()
        {
            var res = FileUtility.GetAllPossibleRedundant();
            Console.ReadKey();
        }

        private static void Eject()
        {
            EjectHelper eju = new EjectHelper("N:");
            bool result = eju.Eject();
            if (result)
            {
                Console.WriteLine("success");
            }
            else
            {
                Console.WriteLine("fail");
            }
        }

        private static void TestCombine(string folder)
        {
            var files = Directory.GetFiles(folder);

            foreach (var file in files)
            {

            }
        }

        private static void UpdateCache()
        {
            List<string> allPrefix = new List<string>();
            foreach (var name in JavDataBaseManager.GetAllAVId().Select(x => x.ID).ToList())
            {
                if (!allPrefix.Contains(name.Split('-')[0]))
                {
                    allPrefix.Add(name.Split('-')[0]);
                    ScanDataBaseManager.InsertPrefix(name.Split('-')[0]);
                }
            }      
        }

        private static void ConcatVideo()
        {
            var ffmpeg = "c:\\setting\\ffmpeg.exe";
            var folder = "D:\\Test\\";
            var output = "D:\\Test\\output\\";
            var outputFile = "";
            List<string> videos = new List<string>();


            var files = Directory.GetFiles(folder);

            foreach (var file in files)
            {
                FileInfo fi = new FileInfo(file);
                videos.Add(file);

                outputFile = output + "test" + fi.Extension;
            }

            FileUtility.ConcatVideos(videos, outputFile, ffmpeg);
        }

        private static void TestJavLibrarySearch(string id)
        {
            var cc = InitManager.UpdateCookie(null, "http://www.javlibrary.com/cn").CC;

            var res = HtmlManager.GetHtmlContentViaUrl("http://www.javlibrary.com/cn/vl_searchbyid.php?keyword=" + id, "utf-8", true, cc);
        }    

        private static void RemoveDuplicate()
        {
            List<AV> needToFixIds = new List<AV>();
            int invalidCount = 0;
            int tureInvalidCount = 0;
            var invalids = FileUtility.GetInvalidChar();
            var avs = JavDataBaseManager.GetAllAV();

            foreach (var av in avs)
            {
                foreach (var invalid in invalids)
                {
                    if (av.Name.Contains(invalid))
                    {
                        needToFixIds.Add(av);
                    }
                }
            }

            foreach (var id in needToFixIds)
            {
                invalidCount++;
                var effectedRows = avs.Where(x => x.ID == id.ID && x.Company == id.Company && x.Director == id.Director).ToList();

                if (effectedRows.Count == 1)
                {
                    tureInvalidCount++;
                    var target = effectedRows.FirstOrDefault();
                    target.Name = FileUtility.ReplaceInvalidChar(target.Name);

                    JavDataBaseManager.UpdateInvalid(target);
                }
                else if (effectedRows.Count > 1)
                {
                    tureInvalidCount++;
                    int needToFixCount = 0;
                    List<AV> toBeDeleted = new List<AV>();

                    foreach (var item in effectedRows)
                    {
                        foreach (var invalid in invalids)
                        {
                            if (item.Name.Contains(invalid))
                            {
                                needToFixCount++;
                                toBeDeleted.Add(item);
                            }
                        }
                    }

                    if (needToFixCount == effectedRows.Count)
                    {
                        var orderedList = toBeDeleted.OrderBy(x => x.AvId).ToList();
                        var keep = orderedList.FirstOrDefault();
                        keep.Name = FileUtility.ReplaceInvalidChar(keep.Name);

                        JavDataBaseManager.UpdateInvalid(keep);

                        foreach (var delete in toBeDeleted.Skip(1))
                        {
                            JavDataBaseManager.DeleteInvalid(delete);
                        }
                    }
                    else
                    {
                        foreach (var delete in toBeDeleted)
                        {
                            JavDataBaseManager.DeleteInvalid(delete);
                        }
                    }
                }
            }

            Console.WriteLine("Has --> " + invalidCount + " invalid.... trueInvalidCount " + tureInvalidCount);
            Console.ReadKey();
        }

        private static void DownloadFile(string file, string desc)
        {
            Utils.DownloadHelper.DownloadFile(file, desc);
        }

        private static void GetAVFromIdAndName()
        {
            var folder = "E:\\New folder\\Fin";

            var files = Directory.GetFiles(folder);

            int total = files.Length;
            int match = 0;
            List<string> unmatched = new List<string>();

            foreach (var file in files)
            {
                FileInfo fi = new FileInfo(file);
                var split = fi.Name.Split('-');

                if (split.Length >= 3)
                {
                    var id = split[0] + "-" + split[1];
                    var nameAfter = fi.Name.Replace(id, "").Split('-');
                    string name = "";

                    if (nameAfter.Length == 0)
                    {
                        name = fi.Name.Replace(id, "").Replace(fi.Extension, "");
                    }
                    else
                    {
                        name = nameAfter[0];

                        foreach (var slice in nameAfter)
                        {
                            if (slice.Length > 1)
                            {
                                name += "-" + slice;
                            }
                        }
                    }

                    var avs = JavDataBaseManager.GetAllAV(id, name);

                    if (avs != null && avs.Count == 1)
                    {
                        match++;
                    }
                    else
                    {
                        unmatched.Add(fi.FullName);
                    }
                }
            }

            foreach (var unmatch in unmatched)
            {
                Console.WriteLine(unmatch);
            }

            Console.WriteLine("Total: " + total + "  Matched: " + match);
        }

        private static void CheckDuplicate()
        {
            var folder = "E:/New folder/Fin";
            var files = Directory.GetFiles(folder);
            List<FileInfo> fis = new List<FileInfo>();

            foreach (var f in files)
            {
                fis.Add(new FileInfo(f));
            }

            foreach (var fi in fis)
            {

            }
        }
    }
}