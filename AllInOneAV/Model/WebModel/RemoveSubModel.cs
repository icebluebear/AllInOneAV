using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.WebModel
{
    public class RemoveSubModel
    {
        public string SrcFile { get; set; }
        public double SrcFileSize { get; set; }
        public string ScrFileSizeStr { get; set; }
        public string DescFile { get; set; }
        public bool IsDuplicate { get; set; }
    }
}
