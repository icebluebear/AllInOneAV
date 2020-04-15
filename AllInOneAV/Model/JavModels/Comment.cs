using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.JavModels
{
    public class Comments
    {
        public Comments()
        {
            AvID = "";
            AvTitle = "";
            Comment = "";
        }
        public int ID { get; set; }
        public string AvID { get; set; }
        public string AvTitle { get; set; }
        public string Comment { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
