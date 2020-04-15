using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.JavModels
{
    public class Director
    {
        public Director()
        {
            Name = "";
            URL = "";
        }
        public int ID { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
