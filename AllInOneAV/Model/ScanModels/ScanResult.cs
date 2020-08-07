using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ScanModels
{
    public class ScanResult
    {
        public int Id { get; set; }
        public string AvId { get; set; }
        public string AvName { get; set; }
        public string Location { get; set; }
        public string FileName { get; set; }
        public string Director { get; set; }
        public string Publisher { get; set; }
        public string Category { get; set; }
        public string Actress { get; set; }
        public DateTime ReleaseDate { get; set; }

        public string AvFilePath
        {
            get
            {
                return Location + "\\" + FileName;
            }
        }

        public string Prefix
        {
            get
            {
                return AvId.Split('-')[0];
            }
        }

        public List<string> CategoryList
        {
            get
            {
                return Category.Split(',').Where(x => !string.IsNullOrEmpty(x)).ToList();
            }
        }

        public List<string> ActressList
        {
            get
            {
                return Actress.Split(',').Where(x => !string.IsNullOrEmpty(x)).ToList();
            }
        }
    }
}
