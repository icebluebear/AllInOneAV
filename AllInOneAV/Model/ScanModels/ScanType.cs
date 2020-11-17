using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ScanModels
{
    public enum ScanType
    {
        ByCate = 1,
        ByPage = 2,
        ByActress = 3,
        ByFavi = 4,
        ByPrefix = 5,
    }

    public class ScanMap
    { 
        public string Title { get; set; }
        public string Url { get; set; }
    }
}
