using Model.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ScanModels
{
    public class RemoteScanMag
    {
        public int RemoteScanMagId { get; set; }
        public string AvId { get; set; }
        public string AvName { get; set; }
        public string AvUrl { get; set; }
        public string MagTitle { get; set; }
        public string MagUrl { get; set; }
        public double MagSize { get; set; }
        public DateTime MagDate { get; set; }
        public DateTime CreateTime { get; set; }
        public int SearchStatus { get; set; }
        public string MatchFile { get; set; }
    }
}
