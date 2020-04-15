using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JavLibraryDownloader.HtmlHelper
{
    public class HtmlManager
    {
        public static HtmlResponse GetHtmlContentViaUrl(string url)
        {
            HtmlResponse res = new HtmlResponse();
            res.Success = false;

            try
            {
                WebRequest request = WebRequest.Create(url);
                request.Timeout = 99999;
                request.Credentials = CredentialCache.DefaultCredentials;
                request.Timeout = 10000;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream, Encoding.UTF8);
                string responseFromServer = reader.ReadToEnd();
                res.Content = responseFromServer;
                reader.Close();
                dataStream.Close();
                response.Close();
            }
            catch (Exception e)
            {
                res.Success = false;
            }

            res.Success = true;
            return res;
        }
    }

    public class HtmlResponse
    {
        public bool Success { get; set; }
        public string Content { get; set; }
    }
}
