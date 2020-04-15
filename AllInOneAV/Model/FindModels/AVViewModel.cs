using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Model.FindModels
{
    public class AVViewModel
    {
        public string AvId { get; set; }
        public string FileName { get; set; }
        public string Location { get; set; }
        public int AvLength { get; set; }
        public string Category { get; set; }
        public string Actress { get; set; }
        public string Director { get; set; }
        public string Company { get; set; }
        public string Publisher { get; set; }
        public string Name { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Url { get; set; }
        public double FileLength { get; set; }
        public string FileSize { get; set; }
        public string Img { get; set; }
        public string FileLocation { get; set; }
    }
}