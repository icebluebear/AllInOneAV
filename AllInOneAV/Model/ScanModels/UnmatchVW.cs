using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Model.ScanModels
{
    public class UnmatchVW
    {
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string FileSize { get; set; }
        public string FileExts { get; set; }
        public bool HasPlayed { get; set; }
    }
}