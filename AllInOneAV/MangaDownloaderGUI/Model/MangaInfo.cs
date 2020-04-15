using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MangaDownloaderGUI.Model
{
    public class MangaInfo
    {
        public string MangaName { get; set; }
        public string MangeUrl { get; set; }
        public int MangaChapters { get; set; }
        public string MangaPic { get; set; }
        public int LastChapter { get; set; }
        public CookieContainer Cc { get; set; }
        public List<string> Urls { get; set; }
    }
}
