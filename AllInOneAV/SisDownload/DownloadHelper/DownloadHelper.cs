using DataBaseManager.SisDataBaseHelper;
using Model.SisModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utils;

namespace SisDownload.DownloadHelper
{
    public class DownloadHelper
    {
        private static string DetailImg = JavINIClass.IniReadValue("Sis", "DetailImg");
        private static string DetailAttach = JavINIClass.IniReadValue("Sis", "DetailAttach");
        private static string DetailAttachPrefix = JavINIClass.IniReadValue("Sis", "DetailAttachPrefix");
        private static string RootFolder = JavINIClass.IniReadValue("Sis", "root");
        private static string Prefix = JavINIClass.IniReadValue("Sis", "Prefix");
        private static string wholePrefix = JavINIClass.IniReadValue("Sis", "Prefix") + JavINIClass.IniReadValue("Sis", "Forum");

        public static DateTime Start(StringBuilder sb)
        {
            var targets = SisDataBaseManager.GetScanThread().Where(x => x.IsDownloaded != 1).OrderBy(x => x.Channel);
            int current = 1;
            int total = targets.Count();

            sb.AppendLine(string.Format("SisDownload下载总共有{0}帖子", targets.Count()));

            DateTime res = DateTime.Today;

            foreach(var item in targets)
            {
                sb.AppendLine(string.Format("开始下载{0}", item.Url));
                res = DoDownload(item, sb);
                Console.WriteLine(string.Format("{0} / {1}", current, total));
                current++;
            }

            return res;
        }

        public static DateTime DoDownload(ScanThread st, StringBuilder sb)
        {
            return DoParse(st, sb);
        }

        public static DateTime DoParse(ScanThread st, StringBuilder sb)
        {
            var url = st.Url;
            var res = HtmlManager.GetHtmlContentViaUrl(url, "gbk");
            DateTime today = DateTime.Today;

            if (res.Success)
            {
                sb.AppendLine(string.Format("    获取内容成功"));
                string subFolder = today.ToString("yyyy年MM月dd日") + "/" + st.Channel + "/";

                if (!string.IsNullOrEmpty(res.Content))
                {
                    var m = Regex.Matches(res.Content, DetailAttach, RegexOptions.Multiline | RegexOptions.IgnoreCase);
                    var attachFolder = "";
                    var attachName = "";
                    var innerSubFolder = "";

                    foreach (Match item in m)
                    {
                        attachFolder = FileUtility.ReplaceInvalidChar(st.Name);
                        attachName = FileUtility.ReplaceInvalidChar(st.Name) + ".torrent";

                        innerSubFolder = subFolder + attachFolder + "/";

                        if (!Directory.Exists(RootFolder + innerSubFolder))
                        {
                            Directory.CreateDirectory(RootFolder + innerSubFolder);
                        }

                        var attach = Prefix + DetailAttachPrefix + item.Groups[1].Value + "&clickDownload=1";
                        var path = RootFolder + subFolder + attachName;

                        //Console.WriteLine(string.Format("Download {0} to {1} and create folder {2} for picture", attach, path, innerSubFolder));
                        sb.AppendLine(string.Format("    Download {0} to {1} and create folder {2} for picture", attach, path, innerSubFolder));

                        if (!string.IsNullOrEmpty(Utils.DownloadHelper.DownloadFile(attach, path)))
                        {
                            sb.AppendLine(string.Format("    下载附件失败"));
                        }

                        var ps = Regex.Matches(res.Content, DetailImg, RegexOptions.Multiline | RegexOptions.IgnoreCase);
                        int index = 1;

                        foreach (Match p in ps)
                        {
                            if (p.Groups[1].Value.ToLower().StartsWith("http"))
                            {
                                var pic = p.Groups[1].Value;
                                var picPath = RootFolder + innerSubFolder + index + ".jpg";
                                //Console.WriteLine(string.Format("Download Picture {0} to {1}", pic, picPath));
                                sb.AppendLine(string.Format("    Download Picture {0} to {1}", pic, picPath));
                                if (string.IsNullOrEmpty(Utils.DownloadHelper.DownloadFile(pic, picPath)))
                                {
                                    sb.AppendLine(string.Format("    下载图片失败"));
                                }
                                Console.Write(".");
                                index++;
                            }
                        }

                        sb.AppendLine(string.Format("    更新{0}的下载状态", url));
                        SisDataBaseManager.UpdateDownload(url);
                        Console.WriteLine();
                    }
                }
            }
            else
            {
                sb.AppendLine(string.Format("    获取{0}内容失败", st.Url));
            }

            sb.AppendLine("*******************************************************************************");

            return today;
        }
    }
}