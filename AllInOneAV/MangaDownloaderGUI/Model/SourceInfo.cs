using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangaDownloaderGUI.Model
{
    public class SourceInfo
    {
        public string SourceName { get; set; }
        public string SourceUrl { get; set; }
        public string SourceHost { get; set; }
        public string SourceReffer { get; set; }
        public string SourceSearch { get; set; }

        public override string ToString()
        {
            return this.SourceName;
        }
    }
}
