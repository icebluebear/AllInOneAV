using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.SisModels
{
    public class SisLogger
    {
        public string Channel { get; set; }
        public string URL { get; set; }
        public string LogContent { get; set; }
        public string Operation { get; set; }
        public int Exception { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
