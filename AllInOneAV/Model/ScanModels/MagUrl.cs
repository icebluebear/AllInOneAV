using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ScanModels
{
    public class MagUrlModel
    {
        public int MagUrlId { get; set; }
        public string AvId { get; set; }
        public string MagUrl { get; set; }
        public string MagTitle { get; set; }
        public bool IsFound { get; set; }
    }
}
