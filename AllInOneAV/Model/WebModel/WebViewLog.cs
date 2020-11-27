using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.WebModel
{
    public class WebViewLog
    {
        public int WebViewLogId { get; set; }
        public string IPAddress { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Parameter { get; set; }
        public string UserAgent { get; set; }
        public int IsLogin { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
