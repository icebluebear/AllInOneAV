using DataBaseManager.JavDataBaseHelper;
using DataBaseManager.LogHelper;
using Model.JavModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utils;

namespace JavLibraryDownloader.ScanHelper
{
    public class ScanManager
    {
        private static JavLibraryLog _logger = JavLoggerManager.GetScanLogger();
        private static string prefix = JavINIClass.IniReadValue("Jav", "prefix");
        private static string listPattern = JavINIClass.IniReadValue("Jav", "listPattern");
        private static string listPageNext = JavINIClass.IniReadValue("Jav", "listPageNext");
        private static string listLastPage = JavINIClass.IniReadValue("Jav", "listLastPage");
        private static string updatePageNext = JavINIClass.IniReadValue("Jav", "updatePageNext");
        private static string updateLastPage = JavINIClass.IniReadValue("Jav", "updateLastPage");
        private static string RootFolder = JavINIClass.IniReadValue("Sis", "root");
        private static List<ScanURL> ForUpdate = new List<ScanURL>();

        public static CookieContainer Scan(string url, string category, int currentCategory, int totalCategories, CookieContainer cc, bool isUpdate = false)
        {
            try
            {
                int page = 1;

                while (!string.IsNullOrEmpty(url))
                {
                    var ret = RecursiveHelper(url, category, currentCategory, totalCategories, page, cc, isUpdate);
                    url = ret.Url;
                    cc = ret.Cc;

                    page++;
                }

                //var json = JsonConvert.SerializeObject(ForUpdate);
                var noDownloads = JavDataBaseManager.GetScanURL().Where(x => x.IsDownload == false);

                int current = 1;

                if (isUpdate)
                {
                    foreach (var update in ForUpdate)
                    {
                        if (!JavDataBaseManager.HasScan(update))
                        {
                            cc = DownloadHelper.DownloadManager.Download(update.URL, current, ForUpdate.Count, cc);
                        }
                        current++;
                    }
                }
                else
                {
                    foreach (var download in noDownloads)
                    {
                        if (!JavDataBaseManager.HasScan(download))
                        {
                            cc = DownloadHelper.DownloadManager.Download(download.URL, current, noDownloads.Count(), cc);
                        }
                        current++;
                    }
                }
            }
            catch (Exception e)
            {
                _logger.WriteExceptionLog("", string.Format("Scan failed. {0}", e.ToString()));           
            }

            return cc;
        }

        public static RecurModel RecursiveHelper(string url, string category, int currentCategory, int totalCategories, int currentPage, CookieContainer cc, bool isUpdate = false)
        {
            try
            {
                var ret = InitHelper.InitManager.UpdateCookie(cc, url);
                cc = ret.CC;
                var res = ret.Content;
                List<ScanURL> temp = new List<ScanURL>();
                int totalPage = currentPage;

                if (res.Success)
                {
                    MatchCollection m = null;

                    if (isUpdate)
                    {
                        m = Regex.Matches(res.Content, updateLastPage, RegexOptions.Multiline | RegexOptions.IgnoreCase);
                    }
                    else
                    {
                        m = Regex.Matches(res.Content, listLastPage, RegexOptions.Multiline | RegexOptions.IgnoreCase);
                    }

                    if (m.Count > 0)
                    {
                        Match first = m[0];
                        var str = first.Groups[1].Value.Replace("\\\">", "");
                        totalPage = int.Parse(str.Substring(str.LastIndexOf("=") + 1));
                    }

                    Console.WriteLine(string.Format("Start process list of {0}, page {1}/{2}, categories {3}/{4}", category, currentPage, totalPage, currentCategory, totalCategories));

                    if (res.Success)
                    {
                        m = Regex.Matches(res.Content, listPattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);

                        foreach (Match item in m)
                        {
                            ScanURL s = new ScanURL
                            {
                                Category = category,
                                CreateTime = DateTime.Now,
                                ID = item.Groups[3].Value,
                                IsDownload = false,
                                Title = FileUtility.ReplaceInvalidChar(item.Groups[2].Value.Replace(item.Groups[3].Value + " ", "")),
                                URL = item.Groups[1].Value
                            };

                            temp.Add(s);
                        }

                        foreach (var scan in temp)
                        {
                            if (!JavDataBaseManager.HasScan(scan))
                            {
                                JavDataBaseManager.InsertScanURL(scan);
                            }
                        }

                        if (isUpdate)
                        {
                            m = Regex.Matches(res.Content, updatePageNext, RegexOptions.Multiline | RegexOptions.IgnoreCase);
                        }
                        else
                        {
                            m = Regex.Matches(res.Content, listPageNext, RegexOptions.Multiline | RegexOptions.IgnoreCase);
                        }

                        if (m.Count > 0)
                        {
                            Match first = m[0];

                            if (isUpdate)
                            {
                                ForUpdate.AddRange(temp);
                            }

                            return new RecurModel{
                                Url = prefix + first.Groups[1].Value,
                                Cc = cc
                            };
                        }
                        else
                        {
                            return new RecurModel
                            {
                                Url = "",
                                Cc=cc
                            };
                        }
                    }
                }
                else
                {
                    _logger.WriteExceptionLog(url, string.Format("Scan failed"));
                    return  new RecurModel
                    {
                        Url = "",
                        Cc = cc
                    };
                }
            }
            catch (Exception e)
            {
                _logger.WriteExceptionLog(url, string.Format("Scan error {0}", e.ToString()));
            }
            return new RecurModel
            {
                Url = url,
                Cc = cc
            };
        }
    }
}
