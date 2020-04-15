using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavLibraryDownloader.Models
{
    public class AV
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public string Publisher { get; set; }
        public string Directory { get; set; }
        public string Category { get; set; }
        public string Actress { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int AvLength { get; set; }
        public DateTime CreateTime { get; set; }
        public string PictureURL { get; set; }
        public string URL { get; set; }
    }
}
