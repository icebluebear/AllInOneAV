using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BookDownloader
{
    class Program
    {
        static void Main(string[] args)
        {
            int mode = 0;
            string defaultRoot = "c:/comic/";
            Console.WriteLine("欢迎使用“士绅漫画网”下载器 v1.0");
            Console.WriteLine("请登录 http://www.ssmh.cc/ 查看详情");
            Console.WriteLine("1 下载单部");
            Console.WriteLine("2 下载全部");
            Console.WriteLine("请输入 1 或 2 + 回车进行选择");

            int.TryParse(Console.ReadLine(), out mode);

            while(mode != 1 && mode != 2)
            {
                Console.WriteLine("输入有误重新输入");
                int.TryParse(Console.ReadLine(), out mode);
            }

            if (mode == 1)
            {
                var url = "";
                Console.WriteLine("请输入想要下载的漫画网址，例如《养女》的网址为http://www.ssmh.cc/book/4749");
                url = Console.ReadLine();

                while (string.IsNullOrEmpty(url))
                {
                    Console.WriteLine("请输入想要下载的漫画网址，例如《养女》的网址为http://www.ssmh.cc/book/4749");
                    url = Console.ReadLine();
                }

                if (!url.Contains("http://www.ssmh.cc/book/"))
                {
                    Console.WriteLine("输入有误，你是不是不想好好下载？！");
                    Console.WriteLine("按任意键退出");
                    Console.ReadKey();
                    return;
                }

                Console.WriteLine("请输入默认保存漫画目录，默认 " + defaultRoot);
                var inputRoot = Console.ReadLine();

                if (!string.IsNullOrEmpty(inputRoot))
                {
                    if (inputRoot.EndsWith("/"))
                    {
                        defaultRoot = inputRoot;
                    }
                    else
                    {
                        defaultRoot = inputRoot + "/";
                    }
                }

                DownloadSingle(url, defaultRoot);
            }
            else
            {
                Console.WriteLine("请输入默认保存漫画目录，默认 " + defaultRoot);
                var inputRoot = Console.ReadLine();

                if (!string.IsNullOrEmpty(inputRoot))
                {
                    if (inputRoot.EndsWith("/"))
                    {
                        defaultRoot = inputRoot;
                    }
                    else
                    {
                        defaultRoot = inputRoot + "/";
                    }
                }

                DownoadAll(defaultRoot);
            }
        }

        public static void DownloadSingle(string index, string root, bool keepOld = true)
        {
            var host = "http://www.ssmh.cc/";
            List<PicToDownload> total = new List<PicToDownload>();

            Console.WriteLine(string.Format("首页 -> {0}, 根目录 -> {1}, 主机 -> {2}", index, root, host));

            var bookInfo = Helper.GetAllChapters(index);

            if (!string.IsNullOrEmpty(bookInfo.BookName) && bookInfo.Chapters.Count > 0)
            {
                if (!Directory.Exists(root))
                {
                    Directory.CreateDirectory(root);
                }

                foreach (var c in bookInfo.Chapters)
                {
                    total.AddRange(Helper.DownloadChapter(host, root, bookInfo.BookName, c));
                }

                var jsonFile = Helper.GenerateJsonFile(total, bookInfo, root);

                bool isContinue = true;

                while (isContinue)
                {
                    isContinue = Helper.Download(jsonFile, keepOld);
                }
            }
        }

        public static void DownoadAll(string root, bool keepOld = true)
        {
            var host = "http://www.ssmh.cc/";
            var bookList = "http://www.ssmh.cc/booklist/";

            List<BookInfo> allBooks = new List<BookInfo>();
            List<string> list = new List<string>();
            List<PicToDownload> total = new List<PicToDownload>();

            var totalPage = Helper.GetTotalPages(bookList);

            for (int i = 0; i < totalPage; i++)
            {
                list.AddRange(Helper.GetAllBookIndex(bookList, host, (i + 1)));
            }

            foreach (var book in list)
            {
                var bookInfo = Helper.GetAllChapters(book);

                if (!string.IsNullOrEmpty(bookInfo.BookName) && bookInfo.Chapters.Count > 0)
                {
                    if (!Directory.Exists(root))
                    {
                        Directory.CreateDirectory(root);
                    }

                    foreach (var c in bookInfo.Chapters)
                    {
                        total.AddRange(Helper.DownloadChapter(host, root, bookInfo.BookName, c));
                    }

                    var jsonFile = Helper.GenerateJsonFile(total, bookInfo, root);

                    bool isContinue = true;

                    while (isContinue)
                    {
                        isContinue = Helper.Download(jsonFile, keepOld);
                    }
                }

                total = new List<PicToDownload>();
            }
        }
    }
}
