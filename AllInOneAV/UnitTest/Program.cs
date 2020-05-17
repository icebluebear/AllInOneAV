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
using Newtonsoft.Json.Linq;
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
using WebService;

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
        private static readonly string imageFolder = JavINIClass.IniReadValue("Jav", "imgFolder");
        private static readonly Dictionary<string, string> ChannelMapping = new Dictionary<string, string> { { AsiaCensoredAuthorshipSeed, "亚洲有码原创" }, { AsiaCensoredSection, "亚洲有码转帖" }, { WesternUncensoredAuthorshipSeed, "欧美无码原创" }, { WesternUncensored, "欧美无码转帖" }, { AsiaUncensoredAuthorshipSeed, "亚洲无码原创" }, { AsiaUncensoredSection, "亚洲无码转帖" } };
        #endregion

        private static LockModel lockModel = new LockModel();

        static void Main(string[] args)
        {
            JavLibraryHelper.DoDailyUpdate(200, true);

            Console.ReadKey();
        }

        private static void TestEverything(EverythingSearchEnum type = EverythingSearchEnum.Video)
        {
            var content = "mvsd-029 | mvsd029";

            var res = new EverythingHelper().SearchFile(content, type);
        }

        private static void TestScanMatchResult()
        {
            var res = ScanDataBaseManager.GetMatchScanResult();

            var actressDic = new Dictionary<string, List<ScanResult>>();
            var categoryDic = new Dictionary<string, List<ScanResult>>();
            var prefixDic = res.GroupBy(x => x.Prefix).ToDictionary(x => x.Key, x => x.ToList());

            foreach (var r in res)
            {
                foreach (var category in r.CategoryList)
                {
                    if (categoryDic.ContainsKey(category))
                    {
                        categoryDic[category].Add(r);
                    }
                    else
                    {
                        categoryDic.Add(category, new List<ScanResult> { r });
                    }
                }

                foreach (var actress in r.ActressList)
                {
                    if (actressDic.ContainsKey(actress))
                    {
                        actressDic[actress].Add(r);
                    }
                    else
                    {
                        actressDic.Add(actress, new List<ScanResult> { r });
                    }
                }
            }
        }

        private static void ClearAvFile()
        {
            List<FileInfo> fis = new List<FileInfo>();
            var drivers = Environment.GetLogicalDrives();

            foreach (var dri in drivers)
            {
                var folder = dri + "\\fin\\";

                if (Directory.Exists(folder))
                {
                    var files = Directory.GetFiles(folder);

                    foreach (var file in files)
                    {
                        FileInfo fi = new FileInfo(file);

                        if (FileUtility.HasInvalidChar(fi.Name.Replace(fi.Extension, "")))
                        {
                            //Console.WriteLine(fi.Name);
                            fis.Add(fi);
                        }
                    }
                }
            }

            foreach (var fi in fis)
            {
                var targetPath = fi.Directory + "\\" + FileUtility.ReplaceInvalidChar(fi.Name.Replace(fi.Extension, "")) + fi.Extension;

                Console.WriteLine("Move " + fi.FullName + " to " + targetPath);

                fi.MoveTo(targetPath);
            }
        }

        private static void CheckNameMapping()
        {
            var avs = JavDataBaseManager.GetAllAV();
            var files = new DirectoryInfo(imageFolder).GetFiles();

            //foreach (var av in avs)
            //{
            //    if (!File.Exists(imageFolder + av.ID + av.Name + ".jpg"))
            //    {
            //        var possibleFiles = files.Where(x => x.Name.StartsWith(av.ID)).ToList();

            //        if (possibleFiles != null && possibleFiles.Count == 1 && av.PictureURL != "http:../img/noimagepl.gif")
            //        {
            //            Console.WriteLine(av.ID + " " + av.Name);

            //            foreach (var sub in possibleFiles)
            //            {
            //                //sub.MoveTo(FileUtility.ReplaceInvalidChar(sub.Name.Replace(sub.Extension, "")) + sub.Extension);
            //                //JavDataBaseManager.UpdateAvName(FileUtility.ReplaceInvalidChar(sub.Name.Replace(av.ID,"").Replace(sub.Extension, "")), av.AvId);
            //                Console.WriteLine("\t" + sub.Name);
            //            }
            //        }
            //    }
            //}

            //foreach (var av in avs)
            //{
            //    if (av.Name.Contains(".jpg"))
            //    {
            //        JavDataBaseManager.UpdateAvName(av.Name.Replace(".jpg", ""), av.AvId);
            //    }
            //}

            //JavLibraryHelper.DoListSearch(list, true);

            //var files = new DirectoryInfo(imageFolder).GetFiles();

            //foreach (var file in files)
            //{
            //    if (file.Name.Replace(file.Extension, "").EndsWith(" "))
            //    {
            //        if (!File.Exists(imageFolder + file.Name.Replace(file.Extension, "").Trim() + file.Extension))
            //        {
            //            file.MoveTo(imageFolder + file.Name.Replace(file.Extension, "").Trim() + file.Extension);
            //        }
            //        else
            //        {
            //            file.Delete();
            //        }
            //    }
            //}

            //int fcount = 0;
            //int dcount = 0;

            //foreach (var file in files)
            //{
            //    if (FileUtility.HasInvalidChar(file.Name.Replace(file.Extension, "")))
            //    {
            //        var newFile = imageFolder + FileUtility.ReplaceInvalidChar(file.Name.Replace(file.Extension, "")) + file.Extension;

            //        if (File.Exists(newFile))
            //        {
            //            file.Delete();
            //        }
            //        else
            //        {
            //            file.MoveTo(newFile);
            //            fcount++;
            //        }
            //    }


            //}

            //foreach (var av in avs)
            //{

            //    if (FileUtility.HasInvalidChar(av.Name))
            //    {
            //        var newName = FileUtility.ReplaceInvalidChar(av.Name);

            //        if (JavDataBaseManager.HasAv(av.ID, newName))
            //        {
            //            JavDataBaseManager.DeleteAV(av.AvId);
            //        }
            //        else
            //        {
            //            JavDataBaseManager.UpdateAvName(newName, av.AvId);
            //            dcount++;
            //        }
            //    }
            //}

            //Console.WriteLine("File " + fcount);
            //Console.WriteLine("DB " + dcount);

            Console.WriteLine("完毕");
        }

        private static void Download115(string cookieStr, string url)
        {
            CookieContainer cc = new CookieContainer();

            string[] cookies = cookieStr.Split(';');

            foreach (var cookie in cookies)
            {
                if (cookie.Contains("="))
                {
                    var cookieItemSplit = cookie.Split('=');
                    cc.Add(new Cookie
                    {
                        Name = cookieItemSplit[0].Trim(),
                        Value = cookieItemSplit[1].Trim(),
                        Domain = "115.com"
                    });
                }
            }

            DownloadHelper.DownloadHttpsWithHost(url, "c:\\setting\\test.mp4", "115.com", "");
        }

        private static bool Get115SearchResult(string cookieStr, string content, string host = "115.com", string reffer = "https://115.com/?cid=0&offset=0&mode=wangpan", string ua = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3202.94 Safari/537.36 115Browser/12.0.0")
        {
            bool ret = false;

            CookieContainer cc = new CookieContainer();

            string[] cookies = cookieStr.Split(';');

            foreach (var cookie in cookies)
            {
                if (cookie.Contains("="))
                {
                    var cookieItemSplit = cookie.Split('=');
                    cc.Add(new Cookie
                    {
                        Name = cookieItemSplit[0].Trim(),
                        Value = cookieItemSplit[1].Trim(),
                        Domain = "115.com"
                    });
                }
            }

            var url = string.Format("https://webapi.115.com/files/search?search_value=vdd-095&format=json");
            var htmlRet = HtmlManager.GetHtmlWebClient("https://115.com", url, cc);
            if (htmlRet.Success)
            {
                var data = JObject.Parse(htmlRet.Content);

                if (data.Property("count").HasValues && int.Parse(data.Property("count").Value.ToString()) > 0)
                {
                    ret = true;
                }
            }

            return ret;
        }

        private static List<MissingCheckModel> GetMissingAV()
        {
            return JavLibraryHelper.GetAllRelatedJav("prefix", "RKI").Result;
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