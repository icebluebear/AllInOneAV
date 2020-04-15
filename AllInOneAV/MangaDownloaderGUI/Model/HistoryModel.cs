using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangaDownloaderGUI.Model
{
    public class HistoryModel
    {
        public string Url { get; set; }
        public List<string> DownloadedChapters { get; set; }
    }
}
