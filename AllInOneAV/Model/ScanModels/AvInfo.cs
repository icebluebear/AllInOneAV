using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ScanModels
{
    public class AvInfo
    {
        public string Location { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public double Size { get; set; }
        public bool IsH265 { get; set; }
        public bool IsChinese { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
