using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.MangaModel
{
    public class MangaCategory
    {
        public int MangaCategoryId { get; set; }
        public MangaCategorySourceType SourceType { get; set; }
        public string RootCategory { get; set; }
        public string Category { get; set; }
        public string Url { get; set; }
    }

    public enum MangaCategorySourceType
    { 
        憨憨漫画 = 1,
    }
}
