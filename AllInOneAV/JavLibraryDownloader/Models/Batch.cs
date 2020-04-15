using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavLibraryDownloader.Models
{
    public class Batch : DatabaseModel
    {
        public int BatchID { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
