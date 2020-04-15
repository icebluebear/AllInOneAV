using DataBaseManager.JavDataBaseHelper;
using DataBaseManager.LogHelper;
using Model.JavModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Utils;

namespace JavLibraryDownloader.InitHelper
{
    public class InitManager
    {
        private static JavLibraryLog _logger = JavLoggerManager.GetInitLogger();
        private static string categoryURL = JavINIClass.IniReadValue("Jav", "category");
        private static string prefix = JavINIClass.IniReadValue("Jav", "prefix");
        private static string postfix = JavINIClass.IniReadValue("Jav", "postfix");
        private static string categoryPattern = JavINIClass.IniReadValue("Jav", "categoryPattern");
        private static string categoryPrefix = JavINIClass.IniReadValue("Jav", "categoryPrefix");

        public static CookieContainer GetCookie()
        {
            CookieContainer cc = new CookieContainer();
            var process = Broswer.OpenBrowserUrl("http://www.javlibrary.com/cn/");
            Thread.Sleep(5000);
            Broswer.Refresh_click();
            Thread.Sleep(15000);
            Broswer.CloseBroswer();

            var data = new ChromeCookieReader().ReadCookies("javlibrary");

            foreach (var item in data.Where(x => !x.Value.Contains(",")).Distinct())
            {
                Cookie c = new Cookie(item.Name, item.Value, "/", "www.javlibrary.com");

                cc.Add(c);
            }

            cc.Add(new Cookie("over18", "18", "/", "www.javlibrary.com"));
            return cc;
        }

        public static bool InitCategory(CookieContainer cc)
        {
            try
            {


                return GetCategory(cc);
            }
            catch (Exception e)
            {
                _logger.WriteExceptionLog("", string.Format("Init failed. {0}", e.ToString()));
            }

            return false;
        }

        public static ReturnModel UpdateCookie(CookieContainer cc, string url)
        {
            if (cc == null)
            {
                cc = new CookieContainer();

                var data = new ChromeCookieReader().ReadCookies("javlibrary");

                foreach (var item in data.Where(x => !x.Value.Contains(",")).Distinct())
                {
                    Cookie c = new Cookie(item.Name, item.Value, "/", "www.javlibrary.com");

                    cc.Add(c);
                }

                cc.Add(new Cookie("over18", "18", "/", "www.javlibrary.com"));
            }

            var need = HtmlManager.NeedToUpdateCookie(url, "utf-8", true, cc);

            while (need.Need)
            {
                cc = InitHelper.InitManager.GetCookie();
                need = HtmlManager.NeedToUpdateCookie(url, "utf-8", true, cc);
            }

            ReturnModel ret = new ReturnModel
            {
                Content = new Model.JavModels.HtmlResponse(),

                CC = cc
            };
            ret.Content.Content = need.Content.Content;
            ret.Content.Success = need.Content.Success;

            return ret;
        }

        private static bool GetCategory(CookieContainer cc)
        {
            Console.WriteLine("Start to init catrgories...");

            var ret = UpdateCookie(cc, categoryURL);
            cc = ret.CC;
            var res = ret.Content;

            if (res.Success)
            {
                return ProcessCategory(res.Content);
            }
            else
            {
                _logger.WriteExceptionLog("", string.Format("No category found"));          
            }

            return false;
        }

        private static bool ProcessCategory(string content)
        {
            try
            {
                JavDataBaseManager.DeleteCategory();

                var m = Regex.Matches(content, categoryPattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);

                foreach (Match item in m)
                {
                    Category c = new Category
                    {
                        Url = prefix + categoryPrefix + item.Groups[1].Value + postfix,
                        Name = item.Groups[2].Value
                    };

                    Console.WriteLine(string.Format("Get category {0}, URL {1}", c.Name, c.Url));
                    JavDataBaseManager.InsertCategory(c);
                }
            }
            catch (Exception e)
            {
                _logger.WriteExceptionLog("", string.Format("Process category error {0}", e.ToString()));
                return false;
            }

            return true;
        }
    }
}
