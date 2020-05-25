using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace Service
{
    public class OneOneFiveService
    {
        public static bool Get115SearchResult(string cookieStr, string content, string host = "115.com", string reffer = "https://115.com/?cid=0&offset=0&mode=wangpan", string ua = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3202.94 Safari/537.36 115Browser/12.0.0")
        {
            bool ret = false;

            CookieContainer cc = Get115Cookie();

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

        public static bool Add115MagTask(string cookieStr, string mag, string uid, string sign, string host = "115.com", string reffer = "https://115.com/?cid=1835025974666577373&offset=0&tab=download&mode=wangpan", string ua = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3202.94 Safari/537.36 115Browser/12.0.0")
        {
            bool ret = false;

            CookieContainer cc = Get115Cookie();

            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("url", mag);
            param.Add("sign", sign);
            param.Add("uid", uid);
            param.Add("time", DateTime.Now.ToFileTimeUtc() + "");

            var returnStr = HtmlManager.Post("https://115.com/web/lixian/?ct=lixian&ac=add_task_url", param, cc);

            if (!string.IsNullOrEmpty(returnStr))
            {
                var data = Newtonsoft.Json.Linq.JObject.Parse(returnStr);

                bool.TryParse(data.Property("state").Value.ToString(), out ret);
            }

            return ret;
        }

        public static CookieContainer Get115Cookie()
        {
            CookieContainer cc = new CookieContainer();

            var cookieData = new ChromeCookieReader().ReadCookies("115");

            foreach (var item in cookieData.Where(x => !x.Value.Contains(",")).Distinct())
            {
                Cookie c = new Cookie(item.Name, item.Value, "/", "115.com");

                cc.Add(c);
            }

            return cc;
        }
    }
}
