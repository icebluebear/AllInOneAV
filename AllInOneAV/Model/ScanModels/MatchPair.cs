using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ScanModels
{
    public class MatchPair
    {
        public List<MatchPairItem> Item { get; set; }
    }

    public class MatchPairItem
    {
        public FileInfo Fi { get; set; }
        public HashSet<string> PossibleId { get; set; }
    }
}
