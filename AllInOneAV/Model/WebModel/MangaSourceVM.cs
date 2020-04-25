using AVWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.WebModel
{
    public class MangaSourceVM : VMResult
    {
        public List<MangaSourceItem> Sources { get; set; }
    }

    public class MangaSourceItem
    {
        public string SourceName { get; set; }
        public int SourceType { get; set; }
    }
}
