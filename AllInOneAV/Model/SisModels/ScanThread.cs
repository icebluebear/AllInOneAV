using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.SisModels
{
    public class ScanThread
    {
        public string Channel { get; set; }
        public DateTime ScannedDate { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int IsDownloaded { get; set; }
    }
}
