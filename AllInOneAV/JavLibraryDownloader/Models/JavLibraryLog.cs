using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavLibraryDownloader.Models
{
    public class JavLibraryLog : DatabaseModel
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

            DataBaseHelper.DataBaseManager.WriteLog(this);
        }

        public void WriteExceptionLog(string url, string content)
        {
            this.URL = url;
            this.Content = content;
            this.IsException = true;

            DataBaseHelper.DataBaseManager.WriteLog(this);
        }
    }
}
