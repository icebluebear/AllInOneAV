using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Common
{
    public class AvFile
    {
        public bool IsH265 { get; set; }
        public string AvId { get; set; }
        public string AvName { get; set; }
        public bool IsChinese { get; set; }
        public bool IsConvertFail { get; set; }

    }
}
