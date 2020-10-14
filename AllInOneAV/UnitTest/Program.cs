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
using System.Threading;
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
            var mag = @"magnet:?xt=urn:btih:MBCPKKKLQQWMUCQWQZ6UJDSY5AU56MM5&amp;dn=%5BFHD%2F4.53GB%5D+PPPD-133+%E6%8A%BC%E3%81%97%E3%81%AE%E5%BC%B7%E3%81%84%E7%88%86%E4%B9%B3%E5%A5%B3%E3%81%AE%E7%86%B1%E3%81%8F%E3%81%AD%E3%81%A3%E3%81%A8%E3%82%8A%E4%B8%8B%E5%93%81%E3%81%AA%EF%BC%B3%EF%BC%A5%EF%BC%B8+JULIA+1080P&amp;tr=http%3A%2F%2Fsukebei.tracker.wf%3A8888%2Fannounce&amp;tr=udp%3A%2F%2Fopen.stealth.si%3A80%2Fannounce&amp;tr=udp%3A%2F%2Ftracker.opentrackr.org%3A1337%2Fannounce&amp;tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969%2Fannounce&amp;tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969%2Fannounce

magnet:?xt=urn:btih:7deefdb0ae824e68dfac50d1fb50e63c4c8f87cb&amp;dn=gvh-139+%E3%83%9E%E3%83%9E%E3%81%AE%E3%83%AA%E3%82%A2%E3%83%AB%E6%80%A7%E6%95%99%E8%82%B2+%E5%90%9B%E5%B3%B6%E3%81%BF%E3%81%8A&amp;tr=http%3A%2F%2Fsukebei.tracker.wf%3A8888%2Fannounce&amp;tr=udp%3A%2F%2Fopen.stealth.si%3A80%2Fannounce&amp;tr=udp%3A%2F%2Ftracker.opentrackr.org%3A1337%2Fannounce&amp;tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969%2Fannounce&amp;tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969%2Fannounce

magnet:?xt=urn:btih:889261950514623c0db8968dd676efad4326b223&amp;dn=SVDVD822+SNS%E3%81%A7%E8%A6%8B%E3%81%A4%E3%81%91%E3%81%9F%E7%B4%A0%E4%BA%BA%E5%B7%A8%E4%B9%B3%E5%A8%98%E3%81%AB%E8%86%A3%E5%A5%A5%E3%81%BE%E3%81%A7%E3%83%87%E3%82%AB%E3%83%81%E3%83%B3%E3%81%A7%E6%BF%80%E3%83%94%E3%82%B9%EF%BC%81%E5%88%9D%E3%82%81%E3%81%A6%E3%81%AE%E3%83%9D%E3%83%AB%E3%83%81%E3%82%AA%E3%82%A4%E3%82%AD%E3%81%AB%E9%AC%BC%E6%BD%AE%E6%92%92%E3%81%8D%E6%95%A3%E3%82%89%E3%81%97%E3%82%A8%E3%83%93%E5%8F%8D%E3%82%8A%E7%B5%B6%E9%A0%82%E3%81%99%E3%82%8B%E3%83%89M%E6%95%8F%E6%84%9F%E5%A8%98%E3%80%82&amp;tr=http%3A%2F%2Fsukebei.tracker.wf%3A8888%2Fannounce&amp;tr=udp%3A%2F%2Fopen.stealth.si%3A80%2Fannounce&amp;tr=udp%3A%2F%2Ftracker.opentrackr.org%3A1337%2Fannounce&amp;tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969%2Fannounce&amp;tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969%2Fannounce

magnet:?xt=urn:btih:07a89a9cb69ce19ea7ba6aabfcb134179761b8dc&amp;dn=gmem-015+%E7%8B%82%E6%B0%97%E6%8B%B7%E5%95%8F%E5%87%A6%E5%88%91Episode02%3A%E6%82%AA%E9%AD%94%E3%81%AE%E5%AA%9A%E8%96%AC%E3%81%AB%E6%9A%B4%E5%A5%B3%E6%8D%9C%E6%9F%BB%E5%AE%98%E3%81%8B%E3%81%AA%E3%81%A7%E8%87%AA%E7%94%B1&amp;tr=http%3A%2F%2Fsukebei.tracker.wf%3A8888%2Fannounce&amp;tr=udp%3A%2F%2Fopen.stealth.si%3A80%2Fannounce&amp;tr=udp%3A%2F%2Ftracker.opentrackr.org%3A1337%2Fannounce&amp;tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969%2Fannounce&amp;tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969%2Fannounce";

            var split = mag.Split(new string[] { "magnet:?" }, StringSplitOptions.None).Where(x => !string.IsNullOrEmpty(x));

            Dictionary<string, string> param = new Dictionary<string, string>();

            if (split.Count() <= 1)
            {
                param.Add("url", mag);
            }
            else
            {
                int index = 0;
                foreach (var s in split)
                {
                    param.Add(string.Format("url[{0}]", index), "magnet:?" + s);

                    index++;
                }
            }

            Console.ReadKey();
        }

        private static void Test115Search()
        {
            var cookie = OneOneFiveService.Get115Cookie();
            var result = OneOneFiveService.Get115SearchResult(cookie, "vdd");
        }

        private static void Check115Mathces()
        {
            var cookie = OneOneFiveService.Get115Cookie();
            var drivers = Environment.GetLogicalDrives();

            foreach (var drive in drivers)
            {
                var targetFolder = drive + "\\fin\\";
                
                if (Directory.Exists(targetFolder))
                {
                    var files = new DirectoryInfo(targetFolder).GetFiles();

                    foreach (var file in files)
                    {
                        var searchContent = file.Name.Split('-')[0] + "-" + file.Name.Split('-')[1];

                        var result = OneOneFiveService.Get115SearchResult(cookie, searchContent);

                        if (!result)
                        {
                            Console.WriteLine(file.FullName);
                        }
                    }
                }
            }
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

                     seeds.Add(searchContent, MagService.SearchSukebei(searchContent));
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