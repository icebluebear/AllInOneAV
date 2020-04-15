using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Common
{
    public class ReportModel
    {
        public int Top { get; set; }

        public int TotalRecord { get; set; }

        public int TotalAV { get; set; }

        public int TotalFiles { get; set; }

        public string TotalSizeStr { get; set; }

        public long TotalSizeLong { get; set; }

        public int TotalMatch { get; set; }

        public int TotalUnMatch { get; set; }

        public int AvHasMoreThan1File { get; set; }

        public List<string> NotMatchAnyThing { get; set; }

        public int FileLargeThan1G { get; set; }

        public int FileLargeThan2G { get; set; }

        public int FileLargeThan4G { get; set; }

        public Dictionary<string, int> TopPrefix { get; set; }

        public Dictionary<string, int> TopActress { get; set; }

        public Dictionary<string, int> TopDirctor { get; set; }

        public Dictionary<string, int> TopCategory { get; set; }

        public Dictionary<string, int> TopCompany { get; set; }

        public Dictionary<string, int> TopDate { get; set; }

        public Dictionary<string, int> Formats { get; set; }
    }
}
