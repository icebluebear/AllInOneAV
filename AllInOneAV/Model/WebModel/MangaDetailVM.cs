using AVWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.WebModel
{
    public class MangaDetailVM : VMResult
    {
        public string MangaName { get; set; }
        public string PicUrl { get; set; }
        public string Author { get; set; }
        public string MangaStatus { get; set; }
        public string UpdateInfo { get; set; }
        public DateTime UpdateDate { get; set; }
        public string Description { get; set; }
        public List<MangaChapter> Chapters { get; set; }
    }

    public class MangaChapter
    { 
        public string ChapterName { get; set; }
        public string Url { get; set; }
    }
}
