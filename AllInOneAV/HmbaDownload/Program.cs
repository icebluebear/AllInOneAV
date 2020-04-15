using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Utils;

namespace HmbaDownload
{
    public class Program
    {
        #region parameters
        private static string folder = ConfigurationManager.AppSettings["infofolder"];
        private static string comicFolder = ConfigurationManager.AppSettings["downloadfolder"];

        private static string manhuafenfolder = ConfigurationManager.AppSettings["manhuafeninfofolder"];
        private static string manhuafencomicFolder = ConfigurationManager.AppSettings["manhuafendownloadfolder"];

        private static string zip = ConfigurationManager.AppSettings["zip"];
        private static string webprefix = ConfigurationManager.AppSettings["webprefix"];
        private static string websearch = ConfigurationManager.AppSettings["websearch"];
        private static string webhost = ConfigurationManager.AppSettings["webhost"];
        private static string zipfolder = ConfigurationManager.AppSettings["zipfolder"];
        private static string urlencode = ConfigurationManager.AppSettings["urlencode"];

        private static string manhuafenwebprefix = ConfigurationManager.AppSettings["manhuafenwebprefix"];
        private static string manhuafenwebsearch = ConfigurationManager.AppSettings["manhuafenwebsearch"];
        private static string manhuafenwebhost = ConfigurationManager.AppSettings["manhuafenwebhost"];

        private static string mainthread = ConfigurationManager.AppSettings["picthread"];
        private static string picthread = ConfigurationManager.AppSettings["picthread"];
        #endregion

        public static void Main(string[] args)
        {
            int cate = 0;

            while (cate <= 0 || cate > 2)
            {
                Console.WriteLine("输入序号选择漫画源  1 -> 色色的韩国漫画   2 -> 普通主流漫画");

                int.TryParse(Console.ReadLine(), out cate);
            }

            if (cate == 1)
            {
                ProceeHmbaVip();
            }

            if (cate == 2)
            {
                ProcessManhuafen();
            }

            Console.WriteLine("已完毕...按任意键退出");
            Console.ReadKey();
        }

        #region Manhuafen
        private static void ProcessManhuafen()
        {
            try
            {
                Init();

                int type = 0;
                string[] content = null;

                Console.WriteLine("进入漫画粉下载器");

                Console.WriteLine("当前log文件目录 -> " + manhuafenfolder);
                Console.WriteLine("当前漫画下载目录 -> " + manhuafencomicFolder);
                Console.WriteLine("是否新建压缩文件 -> " + zip);
                Console.WriteLine("压缩文件目录 -> " + zipfolder);
                Console.WriteLine("主下载线程数 -> " + mainthread);
                Console.WriteLine("图片下载线程数 -> " + picthread);
                Console.WriteLine("如需修改请更改config文件, 文件夹需要用'\\'结尾");
                Console.WriteLine("遇到漫画下载不全等问题,请尝试清理log目录 " + manhuafenfolder + " 并重新下载");
                Console.WriteLine("!!!首次使用可能会自动打开Chrome获取Cookie并关闭,请先保存在Chrome中的内容!!!");
                Console.WriteLine();

                while (type < 1 || type > 2)
                {
                    Console.WriteLine("输入序号选择操作 1 -> 下载, 2 -> 搜索(未开发)");

                    int.TryParse(Console.ReadLine(), out type);
                }

                var cc = InitManhuafenCookie();

                if (type == 1)
                {
                    int thread = 1;

                    int.TryParse(mainthread, out thread);

                    while (content == null || content.Length <= 0)
                    {
                        Console.WriteLine("请输入漫画详情页网址: 请去" + manhuafenwebprefix + " 搜索想要漫画, 用','分割多多部漫画, 例如<进击的巨人>输入'https://www.manhuafen.com/comic/39/'");

                        content = Console.ReadLine().Split(',');
                    }

                    Parallel.ForEach(content, new ParallelOptions { MaxDegreeOfParallelism = thread }, c =>
                    {
                        DownloadManhuafenComic(c, cc);
                    });           
                }

                if (type == 2)
                {
                    Console.WriteLine("尚未实现,等待后续版本");
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.ToString());
            }
        }

        private static CookieContainer InitManhuafenCookie()
        {
            CookieContainer cc = new CookieContainer();

            CookieCollection ccol = HtmlManager.GetCookies(manhuafenwebprefix, "utf-8");

            cc.Add(ccol);

            //var data = new ChromeCookieReader().ReadCookies(manhuafenwebprefix.Replace("https://", ""));

            //if (data.Count > 0)
            //{
            //    foreach (var item in data.Where(x => !x.Value.Contains(",")).Distinct())
            //    {
            //        Cookie c = new Cookie(item.Name, item.Value, "/", manhuafenwebprefix.Replace("https://", ""));

            //        cc.Add(c);
            //    }
            //}
            //else
            //{
            //    var process = Broswer.OpenBrowserUrl(manhuafenwebprefix);
            //    Thread.Sleep(3000);
            //    Broswer.CloseBroswer();

            //    data = new ChromeCookieReader().ReadCookies(manhuafenwebprefix.Replace("https://", ""));

            //    foreach (var item in data.Where(x => !x.Value.Contains(",")).Distinct())
            //    {
            //        Cookie c = new Cookie(item.Name, item.Value, "/", manhuafenwebprefix.Replace("https://", ""));

            //        cc.Add(c);
            //    }
            //}

            return cc;
        }

        private static void DownloadManhuafenComic(string path, CookieContainer cc)
        {
            var retUrl = GetEnterUrl(path, cc);

            if (!string.IsNullOrEmpty(retUrl.Item2))
            {
                Console.WriteLine("开始下载 " + retUrl.Item1);

                var comicFile = manhuafencomicFolder + FileUtility.ReplaceInvalidChar(retUrl.Item1);
                Console.WriteLine("生成漫画下载根目录 " + comicFile);
                string comicRoot = InitComic(comicFile);

                Console.WriteLine("初始化log文件");
                var logFile = manhuafenfolder + FileUtility.ReplaceInvalidChar(retUrl.Item1) + ".json";
                GenerateManhuafenLogFile(logFile);

                DownloadRecursively(manhuafenwebprefix + retUrl.Item2, cc, comicRoot, logFile, HtmlManager.GetChromeVersion());

                var needZip = false;

                bool.TryParse(zip, out needZip);

                if (needZip)
                {
                    var targerFile = zipfolder + FileUtility.ReplaceInvalidChar(retUrl.Item1) + ".zip";
                    Console.WriteLine("\t准备压缩文件夹 " + comicRoot + " 到文件 " + targerFile);

                    var res = ZipHelper.Zip(comicRoot, targerFile);

                    if (res)
                    {
                        Console.WriteLine("\t压缩成功");
                    }
                    else
                    {
                        Console.WriteLine("\t压缩失败");
                    }
                }
            }
        }

        private static void DownloadRecursively(string path, CookieContainer cc, string comicRoot, string logFile, string version)
        {
            var record = ReadLog(logFile);

            var picList = ProcessManhuafenComic(path, cc);

            if (picList.Item1.Count >= 2)
            {
                Console.WriteLine(picList.Item1.FirstOrDefault() + " 共解析出 " + (picList.Item1.Count - 1) + " 张图片");

                Console.WriteLine("创建漫画子文件夹 " + comicRoot + "\\" + picList.Item1.FirstOrDefault());

                var subfolder = InitComic(comicRoot + "\\" + picList.Item1.FirstOrDefault());

                if (!record.Record.ContainsKey(subfolder) || (record.Record.ContainsKey(subfolder) && record.Record[subfolder] == false))
                {
                    Console.WriteLine("没有下载过或者之前有错误,开始下载");
                    var realPic = picList.Item1.Skip(1).ToList();

                    int picThread = 1;

                    int.TryParse(picthread, out picThread);

                    Parallel.ForEach(realPic, new ParallelOptions { MaxDegreeOfParallelism = picThread }, pic =>
                    {
                        Console.WriteLine("开始下载 " + picList.Item1.FirstOrDefault() + " 的第 " + (realPic.IndexOf(pic) + 1) + " 张图片");
                        DownloadHelper.DownloadHttps(pic, subfolder + "\\" + (realPic.IndexOf(pic) + 1) + ".jpg", version, 3, 1);
                    });

                    //foreach (var pic in realPic)
                    //{
                    //    Console.WriteLine("开始下载 " + picList.Item1.FirstOrDefault() + " 的第 " + index + " 张图片");
                    //    DownloadHelper.DownloadHttps(pic, subfolder + "\\" + index + ".jpg", version, 3, 1);
                    //    index++;
                    //}

                    if (Directory.GetFiles(subfolder).Length != picList.Item1.Count - 1)
                    {
                        Console.WriteLine("记录有错误日志");
                        if (record.Record.ContainsKey(subfolder))
                        {
                            record.Record[subfolder] = false;
                        }
                        else
                        {
                            record.Record.Add(subfolder, false);
                        }
                    }
                    else
                    {
                        Console.WriteLine("记录无错误日志");
                        if (record.Record.ContainsKey(subfolder))
                        {
                            record.Record[subfolder] = true;
                        }
                        else
                        {
                            record.Record.Add(subfolder, true);
                        }
                    }

                    WriteLog(logFile, record);
                }
                else
                {
                    Console.WriteLine("之前已经现在过");
                }

                if (!string.IsNullOrEmpty(picList.Item2))
                {
                    Console.WriteLine("有下一章节,开始更新");

                    DownloadRecursively(manhuafenwebprefix + picList.Item2, cc, comicRoot, logFile, version);
                }
                else
                {
                    Console.WriteLine("已经是最后章节,结束更新");
                }
            }
        }

        private static ValueTuple<string, string> GetEnterUrl(string path, CookieContainer cc)
        {
            Console.WriteLine("开始解析 " + path);
            ValueTuple<string, string> ret = new ValueTuple<string, string>();

            var htmlRet = HtmlManager.GetHtmlContentViaUrl(path, "utf-8", true, cc);

            if (htmlRet.Success)
            {
                Console.WriteLine("解析成功");
                var enterPath = "//a[@class='beread_btn']";
                var namePath = "//h1";
                HtmlDocument htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(htmlRet.Content);

                var enterNode = htmlDocument.DocumentNode.SelectSingleNode(enterPath);
                var nameNode = htmlDocument.DocumentNode.SelectSingleNode(namePath);

                if (enterNode != null && enterNode.Attributes != null && enterNode.Attributes["href"] != null)
                {
                    ret.Item2 = enterNode.Attributes["href"].Value.Trim();

                    Console.WriteLine("获取正式链接 " + ret.Item2);
                }

                if (nameNode != null)
                {
                    ret.Item1 = nameNode.InnerText.Trim();

                    Console.WriteLine("获取漫画名称 " + ret.Item1);
                }
            }
            else
            {
                Console.WriteLine("解析失败");
            }

            return ret;
        }

        private static ValueTuple<List<string>, string> ProcessManhuafenComic(string path, CookieContainer cc)
        {
            Console.WriteLine("开始获取图片下载列表");

            ValueTuple<List<string>, string> ret = new ValueTuple<List<string>, string>();
            ret.Item1 = new List<string>();

            var htmlRet = HtmlManager.GetHtmlContentViaUrl(path, "utf-8", true, cc);

            if (htmlRet.Success)
            {
                var name = "";
                var subfolder = "//div[@class='head_title']";
                var nextPath = "//span[@class='next']";
                HtmlDocument htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(htmlRet.Content);

                var node = htmlDocument.DocumentNode.SelectSingleNode(subfolder);
                var nextNode = htmlDocument.DocumentNode.SelectSingleNode(nextPath);

                if (node != null)
                {
                    name = FileUtility.ReplaceInvalidChar(node.InnerText.Replace("\n", "").Replace(" ", ""));

                    Console.WriteLine("获取漫画章节信息 " + name);
                }

                if (nextNode != null && nextNode.ChildNodes.Count == 2)
                {
                    ret.Item2 = nextNode.ChildNodes[1].Attributes["href"].Value.Trim();
                }

                Console.WriteLine("开始解密加密信息..");
                var decrypt = htmlRet.Content.Substring(htmlRet.Content.IndexOf("chapterImages = \"") + "chapterImages = \"".Length);
                decrypt = decrypt.Substring(0, decrypt.IndexOf("\""));

                var picPath = htmlRet.Content.Substring(htmlRet.Content.IndexOf("chapterPath = \"") + "chapterPath = \"".Length);
                picPath = picPath.Substring(0, picPath.IndexOf("\""));

                var pics = DecryptHelper.Decrypt("123456781234567G", decrypt).Split(',');

                if (pics.Length > 0)
                {
                    Console.WriteLine("解密成功");

                    ret.Item1.Add(name);

                    foreach (var pic in pics)
                    {
                        if (!string.IsNullOrEmpty(picPath))
                        {
                            ret.Item1.Add("https://img02.eshanyao.com/" + picPath + pic.Replace("[\"", "").Replace("\"", "").Replace("]", ""));
                        }
                        else
                        {
                            ///"https://img01.eshanyao.com/showImage.php?url="+
                            ret.Item1.Add(pic.Replace("[\"", "").Replace("\"", "").Replace("]", "").Replace("\\",""));
                        }
                    }
                }
                else
                {
                    Console.WriteLine("解密失败");
                }
            }
            else
            {
                Console.WriteLine("获取图片失败");
            }

            return ret;
        }

        private static void GenerateManhuafenLogFile(string name)
        {
            if (!File.Exists(name))
            {
                File.Create(name).Close();

                StreamWriter sw = new StreamWriter(name);

                ManhuafenInfoClass r = new ManhuafenInfoClass();
                r.Record = new Dictionary<string, bool>();

                sw.WriteLine(JsonConvert.SerializeObject(r));

                sw.Close();
            }
        }

        private static ManhuafenInfoClass ReadLog(string name)
        {
            using (StreamReader sr = new StreamReader(name))
            {
                return JsonConvert.DeserializeObject<ManhuafenInfoClass>(sr.ReadToEnd());
            }      
        }

        private static void WriteLog(string name, ManhuafenInfoClass record)
        {
            using (StreamWriter sw = new StreamWriter(name))
            {
                sw.WriteLine(JsonConvert.SerializeObject(record));
            }
        }
        #endregion

        #region Hmbavip
        private static void ProceeHmbaVip()
        {
            try
            {
                Console.WriteLine("进入韩漫吧下载器");

                Init();
                int type = 0;
                string[] content = null;

                Console.WriteLine("当前log文件目录 -> " + folder);
                Console.WriteLine("当前漫画下载目录 -> " + comicFolder);
                Console.WriteLine("是否新建压缩文件 -> " + zip);
                Console.WriteLine("压缩文件目录 -> " + zipfolder);
                Console.WriteLine("主下载线程数 -> " + mainthread);
                Console.WriteLine("图片下载线程数(对韩漫吧不起作用) -> " + picthread);
                Console.WriteLine("如需修改请更改config文件, 文件夹需要用'\\'结尾");
                Console.WriteLine("遇到漫画下载不全等问题,请尝试清理log目录 " + folder + " 并重新下载");
                Console.WriteLine("");

                while (type != 1 && type != 2 && type != 3)
                {
                    Console.WriteLine("输入操作类型, 1 搜索, 2 下载, 3 下载全部(慎用,测试功能)");

                    int.TryParse(Console.ReadLine(), out type);
                }

                var cc = GetCookie(webprefix);

                if (type == 1)
                {
                    while (content == null || content.Length == 0)
                    {
                        Console.WriteLine("请输入搜索内容: ");

                        content = Console.ReadLine().Split(',');
                    }

                    var result = Search(content[0], cc);

                    foreach (var res in result)
                    {
                        Console.WriteLine(string.Format("序号: {0}, {1}", res.Index, res.Title));
                    }

                    if (result.Count > 0)
                    {
                        int index = -1;

                        while (index <= -1 || index > result.Count)
                        {
                            Console.WriteLine("输入需要下载的序号加回车下载");

                            int.TryParse(Console.ReadLine(), out index);
                        }

                        DownloadComic(result[index - 1].Url, cc);
                    }
                    else
                    {
                        Console.WriteLine("没有要搜索的内容");
                    }
                }

                if (type == 2)
                {
                    int thread = 1;

                    int.TryParse(mainthread, out thread);

                    while (content == null || content.Length <= 0)
                    {
                        Console.WriteLine("请输入漫画详情页网址: 请去" + webprefix + " 搜索想要漫画, 用','分割多多部漫画, 例如'http://www.hmba.vip/0_89.html'");

                        content = Console.ReadLine().Split(',');
                    }

                    Parallel.ForEach(content, new ParallelOptions { MaxDegreeOfParallelism = thread }, c =>
                     {
                         DownloadComic(c, cc);
                     });
                }

                if (type == 3)
                {
                    for (int i = 0; i <= 1; i++)
                    {
                        for (int j = i * 1000 + 1; j <= i * 1000 + 999; j++)
                        {
                            var path = "http://www.hmba.vip/" + i + "_" + j + ".html";

                            DownloadComic(path, cc);
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.ToString());
            }
        }

        private static List<ShowInfo> Search(string content, CookieContainer cc)
        {
            var url = websearch;
            Dictionary<string, string> para = new Dictionary<string, string>();
            para.Add("action", "search");
            para.Add("q", content);

            var htmlRet = HtmlManager.Post(url, para);

            var router = ProceeSearch(htmlRet);

            return router;
        }

        private static bool DownloadComic(string path, CookieContainer cc)
        {
            var startTime = DateTime.Now;
            int chapterUpdated = 0;
            int picDownloaded = 0;
            Random rnd = new Random();
            bool useUrlencode = false;
            bool.TryParse(urlencode, out useUrlencode);
            bool ret = false;
            var url = path;
            string index = url.Replace(webprefix, "");

            Console.WriteLine("尝试下载 " + path);

            var htmlRet = HtmlManager.GetHtmlContentViaUrl(url, "utf-8", true, cc);

            var input = ReadFile(index);

            if (htmlRet.Success)
            {
                int maxIndex = -1;

                if (!htmlRet.Content.Contains("文章不存在"))
                {
                    var current = NeedUpdate(htmlRet.Content, input);

                    Console.WriteLine("找到文章 " + current.Title + " 上次下载到第 " + current.LastIndex + " 章");

                    if (current.NeedUpdate)
                    {
                        var rootComicFolder = InitComic(comicFolder + FileUtility.ReplaceInvalidChar(current.Title));
                        var updateUrlList = GetUpdateUrls(htmlRet.Content, input.LastIndex);

                        Console.WriteLine("\t需要更新 " + updateUrlList.Count + " 章");

                        foreach (var updateUrl in updateUrlList)
                        {
                            int currentIndex = int.Parse(updateUrl.Substring(updateUrl.LastIndexOf("/") + 1));

                            maxIndex = Math.Max(maxIndex, currentIndex);

                            Console.WriteLine("\t开始更新第 " + currentIndex + " 章");

                            var episode = useUrlencode ? HttpUtility.UrlDecode(currentIndex + "话", Encoding.UTF8) : currentIndex + "话";
                            var subComicFolder = InitComic(rootComicFolder + "\\" + episode);

                            var count = DownloadPageToDir(updateUrl, cc, subComicFolder + "\\", updateUrl);

                            chapterUpdated++;
                            picDownloaded += count;

                            Console.WriteLine("\t下载了 " + count + " 张图片");

                            current.LastIndex = maxIndex;

                            Thread.Sleep(1000 * rnd.Next(3));
                        }

                        UpdateFile(index, current);

                        var needZip = false;

                        bool.TryParse(zip, out needZip);

                        if (needZip)
                        {
                            var targerFile = zipfolder + FileUtility.ReplaceInvalidChar(current.Title) + ".zip";
                            Console.WriteLine("\t准备压缩文件夹 " + rootComicFolder + " 到文件 " + targerFile);

                            var res = ZipHelper.Zip(rootComicFolder, targerFile);

                            if (res)
                            {
                                Console.WriteLine("\t压缩成功");
                            }
                            else
                            {
                                Console.WriteLine("\t压缩失败");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("\t不要更新");
                    }
                }
                else
                {
                    Console.WriteLine("文章不存在");
                }
            }

            var endTime = DateTime.Now;

            Console.WriteLine("耗时 " + Math.Round((endTime - startTime).TotalMinutes, 2) + " 分钟, 共更新了 " + chapterUpdated + " 章, 和 " + picDownloaded + " 张图片");

            return ret;
        }

        private static void Init()
        {
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            if (!Directory.Exists(manhuafenfolder))
            {
                Directory.CreateDirectory(manhuafenfolder);
            }

            if (!Directory.Exists(zipfolder))
            {
                Directory.CreateDirectory(zipfolder);
            }
        }

        private static string InitComic(string folder)
        {
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            return folder;
        }

        private static void UpdateFile(string name, InfoClass input)
        {
            var fileName = folder + name + ".json";

            StreamWriter sw = new StreamWriter(fileName, false);
            var jsonStr = JsonConvert.SerializeObject(input);
            sw.WriteLine(jsonStr);
            sw.Close();
        }

        private static InfoClass ReadFile(string name)
        {
            InfoClass ret = new InfoClass();
            var fileName = folder + name + ".json";

            if (!File.Exists(fileName))
            {
                File.Create(fileName).Close();
            }
            else
            {
                StreamReader sr = new StreamReader(fileName);
                var jsonStr = sr.ReadToEnd();
                sr.Close();

                ret = JsonConvert.DeserializeObject<InfoClass>(jsonStr);

                if (ret == null)
                {
                    ret = new InfoClass();
                }
            }

            return ret;
        }

        private static CookieContainer GetCookie(string path)
        {
            CookieContainer cc = new CookieContainer();
            cc.Add(HtmlManager.GetCookies(path));

            return cc;
        }

        private static InfoClass NeedUpdate(string content, InfoClass input)
        {
            InfoClass current = new InfoClass();

            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(content);
            string picPath = "//div[@id='fmimg']";
            string infoPath = "//div[@id='info']";
            var picNode = htmlDocument.DocumentNode.SelectSingleNode(picPath);
            var infoNode = htmlDocument.DocumentNode.SelectSingleNode(infoPath);

            if (picNode != null)
            {
                current.Title = picNode.ChildNodes[1].Attributes["alt"].Value;
                current.IsFinish = picNode.ChildNodes[3].Attributes["class"].Value == "a" ? true : false;
            }

            if (infoNode != null)
            {
                var dateStr = infoNode.ChildNodes[7].InnerText;
                var lastIndexStr = infoNode.ChildNodes[9].InnerText;

                if (!string.IsNullOrWhiteSpace(dateStr))
                {
                    current.UpdateTime = DateTime.Parse(dateStr.Substring(dateStr.IndexOf("：") + 1));
                    int lastIndex = 0;
                    int.TryParse(lastIndexStr.Substring(dateStr.IndexOf("：") + 1).Replace("第", "").Replace("话", "").Trim(), out lastIndex);
                    current.LastIndex = lastIndex;
                }
            }

            if (!current.IsFinish && (input.UpdateTime < current.UpdateTime || input.LastIndex < current.LastIndex))
            {
                current.NeedUpdate = true;
            }
            else
            {
                current.NeedUpdate = false;
            }

            return current;
        }

        private static List<string> GetUpdateUrls(string content, int lastIndex)
        {
            List<string> ret = new List<string>();
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(content);
            string listPath = "//dd";

            var nodes = htmlDocument.DocumentNode.SelectNodes(listPath);

            foreach (var node in nodes)
            {
                var urlStr = node.ChildNodes[0].Attributes["href"].Value;
                var currentIndex = int.Parse(urlStr.Substring(urlStr.LastIndexOf("/") + 1));

                if (currentIndex > lastIndex)
                {
                    var temp = webprefix + urlStr;
                    if (!ret.Contains(temp))
                    {
                        ret.Add(temp);
                    }                
                }
            }
            
            return ret;
        }

        private static int DownloadPageToDir(string path, CookieContainer cc, string downloadFolder, string reff)
        {
            int ret = 1;
            int retry = 1;

            var htmlRet = HtmlManager.GetHtmlContentViaUrl(path, "utf-8", true, cc);

            if (htmlRet.Success)
            {
                HtmlDocument htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(htmlRet.Content);
                string picPath = "//img";
                var picNodes = htmlDocument.DocumentNode.SelectNodes(picPath);

                if (picNodes == null)
                {
                    while (retry <= 3)
                    {
                        Console.WriteLine("重试 " + retry + " 次");

                        htmlRet = HtmlManager.GetHtmlContentViaUrl(path, "utf-8", true, cc);
                        if (htmlRet.Success)
                        {
                            htmlDocument = new HtmlDocument();
                            htmlDocument.LoadHtml(htmlRet.Content);
                            picNodes = htmlDocument.DocumentNode.SelectNodes(picPath);
                        }

                        retry++;
                    }
                }

                if (picNodes != null)
                {
                    foreach (var p in picNodes)
                    {
                        if (p.Attributes != null && p.Attributes["src"] != null && !string.IsNullOrEmpty(p.Attributes["src"].Value))
                        {
                            DownloadHelper.DownloadFile(p.Attributes["src"].Value, downloadFolder + ret + ".jpg", webhost, reff);
                            ret++;
                        }
                        else
                        {
                            Console.WriteLine("\t下载图片失败");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("\t下载图片失败");
                }
            }

            return ret;
        }

        private static List<ShowInfo> ProceeSearch(string content)
        {
            //< link rel = "canonical" href = "http://www.hmba.vip/0_567.html" />
            List<ShowInfo> ret = new List<ShowInfo>();
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(content);
            string linkPath = "//link";

            var firstNode = htmlDocument.DocumentNode.SelectSingleNode(linkPath);

            if (firstNode == null)
            {
                
            }
            else
            {
                var linkStr = firstNode.Attributes["href"].Value.Trim();

                if (linkStr == "http://www.hmba.vip/")
                {
                    string itemPath = "//li[@class='item-cover']";
                    var itemNodes = htmlDocument.DocumentNode.SelectNodes(itemPath);
                    int index = 1;

                    foreach (var item in itemNodes)
                    {
                        ret.Add(new ShowInfo
                        {
                            Index = index,
                            Title = item.ChildNodes[1].Attributes["title"].Value.Trim(),
                            Url = "http://www.hmba.vip" + item.ChildNodes[1].Attributes["href"].Value.Trim()
                        }) ;

                        index++;
                    }
                }
                else
                {
                    string picPath = "//div[@id='fmimg']";
                    var picNode = htmlDocument.DocumentNode.SelectSingleNode(picPath);
                    string title = "";

                    if (picNode != null)
                    {
                        title = picNode.ChildNodes[1].Attributes["alt"].Value;
                    }

                    ret.Add(new ShowInfo
                    {
                        Index = 1,
                        Title = title,
                        Url = linkStr
                    });
                }
            }

            return ret;
        }
        #endregion
    }

    class ShowInfo
    { 
        public int Index { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
    }

    class InfoClass
    {
        public string Title { get; set; }
        public DateTime UpdateTime { get; set; }
        public int LastIndex { get; set; }
        public bool IsFinish { get; set; }
        [JsonIgnore]
        public bool NeedUpdate { get; set; }
    }

    class ManhuafenInfoClass
    {
        public Dictionary<string, bool> Record { get; set; }
    }
}
