using DataBaseManager.SisDataBaseHelper;
using Model.SisModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utils;

namespace SisDownload.ScanHelper
{
    public class ScanHelper
    {
        private static string Prefix = JavINIClass.IniReadValue("Sis", "Prefix");
        private static string WholePrefix = JavINIClass.IniReadValue("Sis", "Prefix") + JavINIClass.IniReadValue("Sis", "Forum");
        private static string AsiaUncensoredAuthorshipSeed = Prefix + JavINIClass.IniReadValue("Sis", "AsiaUncensoredAuthorshipSeed");
        private static string AsiaUncensoredSection = Prefix + JavINIClass.IniReadValue("Sis", "AsiaUncensoredSection");
        private static string WesternUncensoredAuthorshipSeed = Prefix + JavINIClass.IniReadValue("Sis", "WesternUncensoredAuthorshipSeed");
        private static string WesternUncensored = Prefix + JavINIClass.IniReadValue("Sis", "WesternUncensored");
        private static string AsiaCensoredAuthorshipSeed = Prefix + JavINIClass.IniReadValue("Sis", "AsiaCensoredAuthorshipSeed");
        private static string AsiaCensoredSection = Prefix + JavINIClass.IniReadValue("Sis", "AsiaCensoredSection");
        private static string RootFolder = JavINIClass.IniReadValue("Sis", "root");
        private static string ListPattern = JavINIClass.IniReadValue("Sis", "ListPattern");
        private static string ListDatePattern = JavINIClass.IniReadValue("Sis", "ListDatePattern");
        private static readonly Dictionary<string, string> ChannelMapping = new Dictionary<string, string> { { AsiaCensoredAuthorshipSeed, "亚洲有码原创" }, { AsiaCensoredSection, "亚洲有码转帖" }, { WesternUncensoredAuthorshipSeed, "欧美无码原创" }, { WesternUncensored, "欧美无码转帖" }, { AsiaUncensoredAuthorshipSeed, "亚洲无码原创" }, { AsiaUncensoredSection, "亚洲无码转帖" } };

        public static void Init(StringBuilder sb)
        {
            if (!Directory.Exists(RootFolder))
            {
                sb.AppendLine(string.Format("没有找到{0},创建文件夹", RootFolder));
                Directory.CreateDirectory(RootFolder);
            }

            var lastOperationEndDate = SisDataBaseManager.GetLastOperationEndDate();
            sb.AppendLine(string.Format("上次执行时间: {0}", lastOperationEndDate.ToString("yyyy-MM-dd")));

            List<string> listChannel = new List<string>();
            //listChannel.Add(AsiaUncensoredAuthorshipSeed);
            //sb.AppendLine(string.Format("添加频道: {0}", "AsiaUncensoredAuthorshipSeed"));
            //listChannel.Add(AsiaUncensoredSection);
            //sb.AppendLine(string.Format("添加频道: {0}", "AsiaUncensoredSection"));
            //listChannel.Add(WesternUncensoredAuthorshipSeed);
            //sb.AppendLine(string.Format("添加频道: {0}", "WesternUncensoredAuthorshipSeed"));
            //listChannel.Add(WesternUncensored);
            //sb.AppendLine(string.Format("添加频道: {0}", "WesternUncensored"));
            listChannel.Add(AsiaCensoredAuthorshipSeed);
            sb.AppendLine(string.Format("添加频道: {0}", "AsiaCensoredAuthorshipSeed"));
            listChannel.Add(AsiaCensoredSection);
            sb.AppendLine(string.Format("添加频道: {0}", "AsiaCensoredSection"));

            foreach (var channel in listChannel)
            {
                var page = 1;

                while (page < 6)
                {
                    var url = string.Format(channel, page);
                    //Console.WriteLine("Get content from " + string.Format(channel, page));
                    sb.AppendLine(string.Format("正在处理URL: {0}, 页码: {1}", url, page));
                    var res = HtmlManager.GetHtmlContentViaUrl(url, "gbk");

                    if (res.Success)
                    {
                        sb.AppendLine("    URL内容获取成功");
                        page = GetTargetThread(res.Content, ChannelMapping[channel], lastOperationEndDate, string.Format(channel, page), sb, page);
                    }
                    else
                    {
                        sb.AppendLine("    URL内容获取失败");
                    }

                    sb.AppendLine("*******************************************************************************");

                    page++;
                }
            }
        }

        private static int GetTargetThread(string content, string channel, DateTime lastDate, string url, StringBuilder sb, int page)
        {
            var ret = ProcessHtml(content, channel, lastDate, url, sb, page);

            return ret;
        }

        private static int ProcessHtml(string content, string channel, DateTime lastDate, string oriUrl, StringBuilder sb, int page)
        {
            if (!string.IsNullOrEmpty(content))
            {
                List<ScanThread> temp = new List<ScanThread>();

                var m = Regex.Matches(content, ListPattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);

                foreach (Match item in m)
                {
                    ScanThread tempItem = new ScanThread
                    {
                        Channel = channel,
                        IsDownloaded = 0,
                        Name = FileUtility.ReplaceInvalidChar(item.Groups[4].Value),
                        Url = Prefix + "thread-" + item.Groups[2].Value + ".html",
                        ScannedDate = DateTime.Now
                    };

                    //Console.WriteLine(string.Format("    Add thread {0} url --> {1}", tempItem.Name, tempItem.Url));
                    sb.AppendLine(string.Format("    Add thread {0} url --> {1}", tempItem.Name, tempItem.Url));
                    temp.Add(tempItem);
                }

                foreach (var item in temp)
                {
                    if (!SisDataBaseManager.IsExistScanThread(item))
                    {
                        SisDataBaseManager.InsertScanThread(item);

                        //Console.WriteLine(string.Format("    插入帖子 {0} of channel {1} url --> {2} 日期 {3}", item.Name, item.Channel, item.Url, item.ScannedDate));
                        sb.AppendLine(string.Format("    插入帖子 {0} of channel {1} url --> {2} 日期 {3}", item.Name, item.Channel, item.Url, item.ScannedDate));
                    }
                    else
                    {
                        sb.AppendLine(string.Format("    已有此贴{0}，不再插入", item.Url));
                    }
                }
            }

            return page++;
        }
    }
}
