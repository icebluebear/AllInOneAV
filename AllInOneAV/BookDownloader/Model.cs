using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace BookDownloader
{
    public class Model
    {
    }

    public class BookInfo
    {
        public string BookName { get; set; }
        public List<Chapter> Chapters { get; set; }
        public CookieContainer Cookie { get; set; }
    }

    public class Chapter
    {
        public string ChapterName { get; set; }
        public string ChapterUrl { get; set; }
    }

    public class PicToDownload
    {
        public string PicUrl { get; set; }
        public string FolderPath { get; set; }
        public string FilePath { get; set; }
        public string Chapter { get; set; }
        public string RootFolder { get; set; }
    }
}
