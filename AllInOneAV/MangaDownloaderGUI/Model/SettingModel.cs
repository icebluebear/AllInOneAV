using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangaDownloaderGUI.Model
{
    public class SettingModel
    {
        public string HistoryFolder { get; set; }
        public string MangaFolder { get; set; }
        public string ZipFolder { get; set; }
        public int ThreadCount { get; set; }
        public bool IsZip { get; set; }
    }
}
