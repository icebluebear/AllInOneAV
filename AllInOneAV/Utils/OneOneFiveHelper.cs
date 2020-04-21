using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class OneOneFiveHelper
    {
        public static bool Get115SearchResult(string cookieStr, string content, string host = "115.com", string reffer = "https://115.com/?cid=0&offset=0&mode=wangpan", string ua = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3202.94 Safari/537.36 115Browser/12.0.0")
        {
            bool ret = false;

            CookieContainer cc = new CookieContainer();

            string[] cookies = cookieStr.Split(';');

            foreach (var cookie in cookies)
            {
                if (cookie.Contains("="))
                {
                    var cookieItemSplit = cookie.Split('=');
                    cc.Add(new Cookie
                    {
                        Name = cookieItemSplit[0].Trim(),
                        Value = cookieItemSplit[1].Trim(),
                        Domain = "115.com"
                    });
                }
            }

            var url = string.Format("https://webapi.115.com/files/search?search_value=vdd-095&format=json");
            var htmlRet = HtmlManager.GetHtmlWebClient("https://115.com", url, cc);
            if (htmlRet.Success)
            {
                var data = Newtonsoft.Json.Linq.JObject.Parse(htmlRet.Content);

                if (data.Property("count").HasValues && int.Parse(data.Property("count").Value.ToString()) > 0)
                {
                    ret = true;
                }
            }

            return ret;
        }
    }
}
