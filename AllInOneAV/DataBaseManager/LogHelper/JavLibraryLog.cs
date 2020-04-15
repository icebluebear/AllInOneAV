using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseManager.LogHelper
{
    public class JavLibraryLog
    {
        public JavLibraryLog()
        {
            Logger = "Default";
            URL = "";
            Content = "";
            IsException = false;
            CreateTime = DateTime.Now;
        }
        public int JavLibraryLogID { get; set; }
        public string Logger { get; set; }
	    public string URL { get; set; }
	    public string Content { get; set; }
	    public bool IsException { get; set; }
        public DateTime CreateTime { get; set; }
        public int BatchID { get; set; }

        public void WriteLog(string url, string content)
        {
            this.URL = url;
            this.Content = content;
            this.IsException = false;
            this.CreateTime = DateTime.Now;

            JavLoggerManager.WriteLog(this);
        }

        public void WriteExceptionLog(string url, string content)
        {
            this.URL = url;
            this.Content = content;
            this.IsException = true;
            this.CreateTime = DateTime.Now;

            JavLoggerManager.WriteLog(this);
        }
    }
}
