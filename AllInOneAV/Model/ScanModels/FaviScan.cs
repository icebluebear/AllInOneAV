using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ScanModels
{
    public class FaviScan
    {
        public int FaviScanId { get; set; }
        public string Category { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public string Avator { get; set; }
        public string Thubnail
        {
            get 
            {
                if (this.Category == "actress")
                {
                    if (!string.IsNullOrEmpty(this.Avator))
                    {
                        return this.Avator;
                    }
                    return "/imgs/avator/default.jpg";
                }
                else
                {
                    //TODO default thubnail
                    return "/imgs/avator/default.jpg";
                }
            }
        }
    }
}
