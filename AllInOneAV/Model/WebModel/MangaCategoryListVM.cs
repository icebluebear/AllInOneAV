using AVWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.WebModel
{
    public class MangaCategoryListVM : VMResult
    {
        public List<MangaCategoryListItem> Mangas { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPage { get; set; }
    }

    public class MangaCategoryListItem
    { 
        public string MangaName { get; set; }
        public string MangaUrl { get; set; }
        public bool IsFinished { get; set; }
        public string UpdateInfo { get; set; }
        public DateTime UpdateDate { get; set; }
        public string PicUrl { get; set; }
    }
}
