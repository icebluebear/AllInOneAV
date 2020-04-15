using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Common
{
    public class CheckDuplcateModel
    {
        public string Key { get; set; }

        public List<string> ContainsFiles { get; set; }

        public bool IsExpend { get; set; }

        public int Biggest { get; set; }
    }
}
