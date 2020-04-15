using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Common
{
    public class SeedMagnetSearchModel
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string MagUrl { get; set; }
        public double Size { get; set; }
        public DateTime Date { get; set; }
        public int CompleteCount { get; set; }
        public SearchSeedSiteEnum Source { get; set; }
    }
}
