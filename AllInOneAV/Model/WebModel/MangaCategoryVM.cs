using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AVWeb.Models
{
    public class MangaCategoryVM : VMResult
    {
        public List<MangaCategoryItem> Categories { get; set; }
    }

    public class MangaCategoryItem
    {
        public string RootCategory { get; set; }
        public string Category { get; set; }
        public string Url { get; set; }
        public bool IsRoot { get; set; }
    }
}