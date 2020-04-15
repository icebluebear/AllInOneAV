using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.JavModels
{
    public class ScanURL
    {
        public ScanURL()
        {
            Category = "";
            URL = "";
            ID = "";
            Title = "";
            IsDownload = false;
            CreateTime = DateTime.Now;
        }
        public string Category { get; set; }
        public string URL { get; set; }
        public string ID { get; set; }
        public string Title { get; set; }
        public bool IsDownload { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
