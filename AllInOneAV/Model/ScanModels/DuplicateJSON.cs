using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ScanModels
{
    public class DuplicateJSON
    {
        public List<DuplicateItem> data { get; set; }
    }

    public class DuplicateItem
    {
        public string AvId { get; set; }
        public List<Match> Matches { get; set; }
    }
}
