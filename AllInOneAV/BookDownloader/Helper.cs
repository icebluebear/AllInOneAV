using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace BookDownloader
{
    public class Helper
    {
        public static BookInfo GetAllChapters(string index)
        {
            Console.WriteLine("获取漫画详情");
            BookInfo ret = new BookInfo();
            ret.Chapters = new List<Chapter>();

            try
            {
                //var cc = new CookieContainer();
                //Console.WriteLine("获取Cookie");
                //var cookies = Utils.HtmlManager.GetCookies(index);

                //ret.Cookie = cc;

                //for (int i = 0; i < cookies.Count; i++)
                //{
                //    cc.Add(cookies[i]);
                //}

                Console.WriteLine("获取详情");
                var content = Utils.HtmlManager.GetHtmlContentViaUrl(index);
                if (content.Success)
                {
                    Console.WriteLine("获取详情成功");
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(content.Content);

                    var titlePath = "//div[@class=\"info\"]/h1";
                    var titleNode = doc.DocumentNode.SelectSingleNode(titlePath);

                    Console.WriteLine("解析标题");
                    if (titleNode != null)
                    {                     
                        var title = titleNode.InnerText;
                        Console.WriteLine("标题 -> " + title);
                        ret.BookName = title;

                        Console.WriteLine("解析章节");
                        var chaptersPath = "//div[@id=\"chapterlistload\"]/ul/li";
                        var chaptersNodes = doc.DocumentNode.SelectNodes(chaptersPath);

                        foreach (var c in chaptersNodes)
                        {
                            Chapter chapter = new Chapter
                            {
                                ChapterName = c.InnerText.Trim().Substring(0, c.InnerText.Trim().LastIndexOf("话") + 1),
                                ChapterUrl = c.ChildNodes["a"].Attributes["href"].Value
                            };

                            Console.WriteLine("章节 -> " + chapter.ChapterName);
                            ret.Chapters.Add(chapter);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("获取详情失败");
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.ToString());
            }

            return ret;
        }

        public static List<PicToDownload> DownloadChapter(string host, string root, string bookName, Chapter chapter)
        {
            Console.WriteLine("处理章节 -> " + chapter.ChapterName);

            List<PicToDownload> ret = new List<PicToDownload>();
            int index = 1;

            var subRoot = root + "/" + bookName + "/" + chapter.ChapterName + "/";

            try
            {
                Console.WriteLine("获取详情");
                var content = Utils.HtmlManager.GetHtmlContentViaUrl(host + chapter.ChapterUrl);
                if (content.Success)
                {
                    Console.WriteLine("获取详情成功");
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(content.Content);

                    Console.WriteLine("解析图片");
                    var picPath = "//img[@class=\"lazy\"]";
                    var picNodes = doc.DocumentNode.SelectNodes(picPath);

                    foreach (var pic in picNodes)
                    {
                        ret.Add(new PicToDownload
                        {
                            PicUrl = pic.Attributes["data-original"].Value,
                            FilePath = subRoot + (index) + ".jpg",
                            FolderPath = subRoot,
                            Chapter = chapter.ChapterName,
                            RootFolder = root + "/" + bookName + "/"
                        });
                        Console.WriteLine("解析图片 -> " + pic.Attributes["data-original"].Value);
                        index++;
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.ToString());
            }

            return ret;
        }

        public static string GenerateJsonFile(List<PicToDownload> total, BookInfo bookInfo, string root)
        {
            Console.WriteLine("处理Json文件");

            var json = JsonConvert.SerializeObject(total);
            var jsonFile = root + "/" + bookInfo.BookName + ".json";

            if (File.Exists(jsonFile))
            {
                Console.WriteLine("删除Json文件");
                File.Delete(jsonFile);
            }
            else
            {
                Console.WriteLine("写入Json文件");
                StreamWriter sw = new StreamWriter(jsonFile);
                sw.WriteLine(json);
                sw.Close();
            }

            return jsonFile;
        }

        public static bool Download(string jsonFile, bool keepOld)
        {
            bool ret = false;
            Console.WriteLine("下载图片");

            Dictionary<string, List<PicToDownload>> picsToCombine = new Dictionary<string, List<PicToDownload>>();
            StreamReader sr = new StreamReader(jsonFile);
            var json = sr.ReadToEnd();
            sr.Close();

            List<PicToDownload> pics = JsonConvert.DeserializeObject<List<PicToDownload>>(json);
            picsToCombine = pics.GroupBy(x => x.Chapter).ToDictionary(x => x.Key, y => y.ToList());

            foreach (var pic in pics)
            {
                try
                {
                    if (!Directory.Exists(pic.FolderPath))
                    {
                        Console.WriteLine("创建文件夹 -> " + pic.FolderPath);
                        Directory.CreateDirectory(pic.FolderPath);
                    }

                    if (!File.Exists(pic.FilePath))
                    {
                        Console.WriteLine("下载图片 -> 从 " + pic.PicUrl + " 到 " + pic.FilePath);
                        Utils.DownloadHelper.DownloadFile(pic.PicUrl, pic.FilePath);
                        ret = true;
                    }
                    else
                    {
                        Console.WriteLine("文件已存在 -> " + pic.FilePath);
                    }
                }
                catch (Exception ee)
                {
                    Console.WriteLine(ee.ToString());
                }
            }

            foreach (var com in picsToCombine)
            {
                var desc = com.Value.FirstOrDefault().RootFolder + com.Value.FirstOrDefault().Chapter + ".jpg";
                Utils.ImageHelper.CombinePics(desc, com.Value.Select(x => x.FilePath).ToList(), keepOld);
            }

            return ret;
        }

        public static int GetTotalPages(string index)
        {
            //var cc = new CookieContainer();
            //Console.WriteLine("获取Cookie");
            //var cookies = Utils.HtmlManager.GetCookies(index);

            var total = 1;
            var pageInfo = "?page=1";
            Console.WriteLine("获取总页数");

            var content = Utils.HtmlManager.GetHtmlContentViaUrl(index + pageInfo);
            if (content.Success)
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(content.Content);

                var totalPagePath = "//div[@class=\"pagination\"]";
                var totalPageNode = doc.DocumentNode.SelectSingleNode(totalPagePath);
                total = int.Parse(totalPageNode.ChildNodes[totalPageNode.ChildNodes.Count - 5].ChildNodes["a"].InnerText);
            }

            return total;
        }

        public static List<string> GetAllBookIndex(string index, string host, int page)
        {
            List<string> ret = new List<string>();
            //var cc = new CookieContainer();
            //Console.WriteLine("获取Cookie");
            //var cookies = Utils.HtmlManager.GetCookies(index);

            var pageInfo = "?page=" + page;
            Console.WriteLine("获取第" + page + "页");

            var content = Utils.HtmlManager.GetHtmlContentViaUrl(index + pageInfo);
            if (content.Success)
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(content.Content);

                var listItemPath = "//div[@class=\"mh-item-detali\"]";
                var listItemNode = doc.DocumentNode.SelectNodes(listItemPath);

                foreach (var node in listItemNode)
                {
                    var temp = node.ChildNodes[1].ChildNodes[1];

                    Console.WriteLine("添加 -> " + host + temp.Attributes["href"].Value);
                    ret.Add(host + temp.Attributes["href"].Value);
                }
            }

            return ret;
        }

        public static void Debug()
        {
            string path = "c:/setting/comic/养女/";

            var folder = Directory.GetDirectories(path);
            int index = 1;

            foreach (var item in folder)
            {
                var files = Directory.GetFiles(item);
                Utils.ImageHelper.CombinePics(path + index + ".jpg", files.ToList(), true);
                index++;
            }
        }
    }
}
