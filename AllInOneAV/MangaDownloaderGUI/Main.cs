using MangaDownloaderGUI.Model;
using MangaDownloaderGUI.Service;
using Model.MangaModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using Utils;

namespace MangaDownloaderGUI
{


    public partial class Main : Form
    {
        public delegate void UpdatePB(ProgressBar pbBar, int value, int max);
        public delegate void UpdateLog(RichTextBox rtb, string content);
        private string UserAgent = JavINIClass.IniReadValue("Html", "UserAgent");
        private List<SourceInfo> sourceList = new List<SourceInfo>();
        private SourceInfo si;
        public static MangaInfo mi;

        public Main()
        {
            InitializeComponent();
        }

        #region 行为

        #region 菜单行为 

        private void Main_Load(object sender, EventArgs e)
        {
            InitUI();
            InitSource();
            InitCbSource();
        }

        private void settingToolStrip_Click(object sender, EventArgs e)
        {
            ShowSetting();
        }

        private void aboutToolStrip_Click(object sender, EventArgs e)
        {
            ShowAbout();
        }

        private void 帮助ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Helper helper = new Helper();
            helper.ShowDialog();
        }

        #endregion

        #region 主页面行为

        private void cbSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (var source in sourceList)
            {
                if (cbSource.Text == source.SourceName)
                {
                    si = source;
                    InitCbDownloaded();
                    break;
                }
            }
        }

        private void cbDownloaded_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtMainSearch.Text = cbDownloaded.Text;

            if (si.SourceName == "韩漫吧")
            {
                Search(false);
            }
            else
            {
                Search();
            }

            AddAdditionalInfo(si);
            ShowManga(si);
        }

        private void btnMainSearch_Click(object sender, EventArgs e)
        {
            Search();
            AddAdditionalInfo(si);
            ShowManga(si);
        }

        private void btnMainDownload_Click(object sender, EventArgs e)
        {
            Download();
        }

        private void rcbLog_ContentsResized(object sender, ContentsResizedEventArgs e)
        {
            rcbLog.SelectionStart = rcbLog.Text.Length;
            rcbLog.ScrollToCaret();
        }
        #endregion

        #endregion

        #region 方法

        #region 菜单方法

        private void InitUI()
        {
            sourceList = new List<SourceInfo>();
            lvwMainList.Items.Clear();
            mi = null;
            si = null;
            cbSource.Text = "请选择";
            cbDownloaded.Text = "无";
            txtMainSearch.Text = "";
            rcbLog.Text = "初始化..." + Environment.NewLine;
            txtMainChapters.Text = "";
            txtMainStatus.Text = "";
            txtMainUrl.Text = "";
            picManga.Image = null;
            pbMain.Value = 0;
            pbMain.Maximum = 0;
            pbMain.Minimum = 0;
        }

        private void InitSource()
        {
            SourceInfo shm = new SourceInfo();
            shm.SourceName = "韩漫吧";
            shm.SourceUrl = "http://www.hmba.vip";
            shm.SourceSearch = "http://www.hmba.vip/home/search";
            shm.SourceReffer = "";
            shm.SourceHost = "http://www.hmba.vip/";

            sourceList.Add(shm);

            SourceInfo shh = new SourceInfo();
            shh.SourceName = "憨憨的漫画";
            shh.SourceUrl = "http://www.hanhande.net";
            shh.SourceSearch = "http://www.hanhande.net/search/?keywords=";
            shh.SourceReffer = "";
            shh.SourceHost = "http://www.hanhande.net/";

            sourceList.Add(shh);

            SourceInfo sww = new SourceInfo();
            sww.SourceName = "污污漫画";
            sww.SourceUrl = "https://www.5wmh.com/";
            sww.SourceSearch = "https://www.5wmh.com/search?keyword=";
            sww.SourceReffer = "";
            sww.SourceHost = "https://www.5wmh.com/";

            sourceList.Add(sww);

            SourceInfo szymk = new SourceInfo();
            szymk.SourceName = "知音漫客";
            szymk.SourceUrl = "http://www.zuozuomanhua.com/";
            szymk.SourceSearch = "http://www.zuozuomanhua.com/search?keyword=";
            szymk.SourceReffer = "";
            szymk.SourceHost = "http://www.zuozuomanhua.com/";

            sourceList.Add(szymk);

            SourceInfo mhdb = new SourceInfo();
            mhdb.SourceName = "漫画DB";
            mhdb.SourceUrl = "https://www.manhuadb.com/";
            mhdb.SourceSearch = "https://www.manhuadb.com/search?q=";
            mhdb.SourceReffer = "";
            mhdb.SourceHost = "https://www.manhuadb.com/";

            sourceList.Add(mhdb);
        }

        private void InitCbSource()
        {
            foreach (var source in sourceList)
            {
                cbSource.Items.Add(source);
            }
        }

        private void InitCbDownloaded()
        {
            if (si != null)
            {
                cbDownloaded.Items.Clear();

                var setting = SettingSevice.ReadSetting();

                if (string.IsNullOrEmpty(setting.HistoryFolder) || (setting.IsZip && string.IsNullOrEmpty(setting.ZipFolder)) || string.IsNullOrEmpty(setting.MangaFolder))
                {
                    MessageBox.Show("请先去设置页面完成设置");
                    return;
                }

                var files = new DirectoryInfo(setting.HistoryFolder).GetFiles();
                foreach (var file in files)
                {
                    if (file.Name.StartsWith(si.SourceName))
                    {
                        cbDownloaded.Items.Add(file.Name.Replace(si.SourceName + "_", "").Replace(".json", ""));
                    }
                }
            }
            else
            {
                cbDownloaded.Text = "请选择";
                cbDownloaded.Items.Clear();
            }
        }

        private void ShowAbout()
        {
            About about = new About();
            about.ShowDialog();
        }

        private void ShowSetting()
        {
            Setting setting = new Setting();
            setting.ShowDialog();
        }
        #endregion

        #region 主页面方法

        public void Search(bool searchList = true)
        {
            if (!string.IsNullOrEmpty(txtMainSearch.Text) && si != null)
            {
                lvwMainList.Items.Clear();

                var list = new List<MangaInfo>();

                list = DoSearch(si.SourceName, txtMainSearch.Text, searchList);

                if (list.Count > 0)
                {
                    MangaList ml = new MangaList(list, si);
                    ml.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("有错误,或者没有输入要搜索的内容");
            }
        }

        public void ShowManga(SourceInfo si)
        {
            DoShowManga(si);
        }

        private void Download()
        {
            if (si != null && mi != null && lvwMainList.SelectedItems.Count > 0)
            {
                DoDownload(si.SourceName);
            }
            else
            {
                MessageBox.Show("有错误,请重新选择漫画下载,或者没有选择要下载的章节.\n请在列表中高亮选择需要下载的章节");
            }
        }

        #endregion

        #region 辅助方法

        private void UpdatePb(ProgressBar jd, int v, int m)
        {
            if (jd.InvokeRequired)
            {
                jd.Invoke(new UpdatePB(UpdatePb), jd, v, m);
            }
            else
            {
                jd.Maximum = m;
                jd.Value = v;
            }
        }

        private void UpdateRtb(RichTextBox rtb, string content)
        {
            if (rtb.InvokeRequired)
            {
                rtb.Invoke(new UpdateLog(UpdateRtb), rtb, content);
            }
            else
            {
                rtb.AppendText(content + Environment.NewLine);
            }
        }

        private List<MangaInfo> DoSearch(string type, string content, bool searchList)
        {
            if (type == "韩漫吧")
            {
                return SearchHmb(content, searchList);
            }

            if (type == "憨憨的漫画")
            {
                return SearchHanhan(content);
            }

            if (type == "污污漫画")
            {
                return SearchWuwu(content);
            }

            if (type == "知音漫客")
            {
                return SearchZhiyinmanke(content);
            }

            if (type == "漫画DB")
            {
                return SearchManhuadb(content);
            }

            return new List<MangaInfo>();
        }

        private void AddAdditionalInfo(SourceInfo si)
        {
            if (mi != null)
            {
                if (si.SourceName == "韩漫吧")
                {
                    AddAddtionalInfoHmb();
                }
                else if (si.SourceName == "憨憨的漫画")
                {
                    AddAddtionalInfoHanhan();
                }
                else if (si.SourceName == "污污漫画")
                {
                    AddAdditionalInfoWuwu();
                }
                else if (si.SourceName == "知音漫客")
                {
                    AddAdditionalInfoZhiyinmanke();
                }
                else if (si.SourceName == "漫画DB")
                {
                    AddAdditionalInfoManhuadb();
                }
            }
        }

        private void DoShowManga(SourceInfo si)
        {
            if (mi != null)
            {
                if (si.SourceName == "韩漫吧")
                {
                    ShowMangeHmb();
                }
                else if (si.SourceName == "憨憨的漫画")
                {
                    ShowMangeHanhan();
                }
                else if (si.SourceName == "污污漫画")
                {
                    ShowMangeWuwu();
                }
                else if (si.SourceName == "知音漫客")
                {
                    ShowMangeZhiyinmanke();
                }
                else if (si.SourceName == "漫画DB")
                {
                    ShowMangeManhuadb();
                }
            }
            else
            {
                MessageBox.Show("没有找到漫画");
            }
        }

        private async void DoDownload(string type)
        {
            var setting = SettingSevice.ReadSetting();
            string root = "";

            if (string.IsNullOrEmpty(setting.HistoryFolder) || (setting.IsZip && string.IsNullOrEmpty(setting.ZipFolder)) || string.IsNullOrEmpty(setting.MangaFolder))
            {
                MessageBox.Show("请先去设置页面完成设置");
                return;
            }

            Dictionary<string, string> downloadUrls = new Dictionary<string, string>();
            foreach (ListViewItem lvi in lvwMainList.SelectedItems)
            {
                downloadUrls.Add((string)lvi.Tag, lvi.Text);
            }

            if (type == "韩漫吧")
            {
                root = await Task.Run(() => DownloadHmb(downloadUrls, setting));
            }

            if (type == "憨憨的漫画")
            {
                root = await Task.Run(() => DownloadHanhan(downloadUrls, setting));
            }

            if (type == "污污漫画")
            {
                root = await Task.Run(() => DownloadWuwu(downloadUrls, setting));
            }

            if (type == "知音漫客")
            {
                root = await Task.Run(() => DownloadZhiyinmanke(downloadUrls, setting));
            }

            if (type == "漫画DB")
            {
                root = await Task.Run(() => DownloadManhuadb(downloadUrls, setting));
            }

            if (!string.IsNullOrEmpty(root))
            {
                if (setting.IsZip)
                {
                    var targerFile = "";

                    if (rbCombine.Checked)
                    {
                        targerFile = setting.ZipFolder + mi.MangaName + "_长图版.zip";
                    }
                    else
                    {
                        targerFile = setting.ZipFolder + mi.MangaName + ".zip";
                    }

                    rcbLog.AppendText("准备压缩文件夹 " + root + " 到文件 " + targerFile + Environment.NewLine);

                    var res = ZipHelper.Zip(root, targerFile);

                    if (res)
                    {
                        rcbLog.AppendText("压缩成功");
                    }
                    else
                    {
                        rcbLog.AppendText("压缩失败");
                    }
                }
            }
        }

        private List<MangaInfo> SearchHmb(string content, bool searchList = true)
        {
            List<MangaInfo> ret = new List<MangaInfo>();
            CookieContainer cc = new CookieContainer();
            cc.Add(HtmlManager.GetCookies(si.SourceUrl));

            rcbLog.AppendText("开始搜索韩漫吧" + Environment.NewLine);

            var searchUrl = si.SourceSearch;
            Dictionary<string, string> para = new Dictionary<string, string>();
            para.Add("action", "search");
            para.Add("q", content);

            var htmlRet = HtmlManager.Post(searchUrl, para);

            HtmlAgilityPack.HtmlDocument htmlDocument = new HtmlAgilityPack.HtmlDocument();
            htmlDocument.LoadHtml(htmlRet);

            //直接搜索到详情页情况
            if (searchList == false)
            {
                var picPath = "//img";
                var utlPath = "//link[@rel='canonical']";

                var varPicNode = htmlDocument.DocumentNode.SelectSingleNode(picPath);
                var urlNode = htmlDocument.DocumentNode.SelectSingleNode(utlPath);

                MangaInfo cmi = new MangaInfo();
                cmi.Urls = new List<string>();
                cmi.MangaName = content;

                if (varPicNode != null)
                {
                    cmi.MangaPic = varPicNode.Attributes["src"].Value.Trim();
                }

                if (urlNode != null)
                {
                    cmi.MangeUrl = urlNode.Attributes["href"].Value.Trim();
                }

                mi = cmi;

                return ret;
            }

            string listPath = "//li[@class='item-cover']";

            var listNodes = htmlDocument.DocumentNode.SelectNodes(listPath);

            if (listNodes != null)
            {
                foreach (var node in listNodes)
                {
                    if (node.ChildNodes.Count > 0)
                    {
                        var aTag = node.ChildNodes.FindFirst("a");

                        if (aTag != null && aTag.ChildNodes.Count > 0)
                        {
                            string pic = "";
                            string title = aTag.Attributes["title"].Value.Trim();
                            string url = si.SourceUrl + aTag.Attributes["href"].Value.Trim();
                            var img = node.ChildNodes.FindFirst("img");

                            if (img != null)
                            {
                                pic = img.Attributes["data-original"].Value.Trim();
                            }

                            ret.Add(new MangaInfo
                            {
                                MangaName = FileUtility.ReplaceInvalidChar(title),
                                MangeUrl = url,
                                MangaPic = pic,
                                Cc = cc,
                                Urls = new List<string>()
                            });
                        }
                    }
                }
            }

            return ret;
        }

        private List<MangaInfo> SearchHanhan(string content)
        {
            List<MangaInfo> ret = new List<MangaInfo>();
            CookieContainer cc = new CookieContainer();
            cc.Add(HtmlManager.GetCookies(si.SourceUrl));

            rcbLog.AppendText("开始搜索憨憨的漫画" + Environment.NewLine);

            var searchUrl = si.SourceSearch;
            content = HttpUtility.UrlEncode(content, Encoding.UTF8);

            var htmlRet = HtmlManager.GetHtmlWebClient(si.SourceHost, searchUrl + content, cc);

            //暂时只搜第一页
            if (htmlRet.Success)
            {
                HtmlAgilityPack.HtmlDocument htmlDocument = new HtmlAgilityPack.HtmlDocument();
                htmlDocument.LoadHtml(htmlRet.Content);

                var itemPath = "//li[@class='item-lg']";

                var itemNodes = htmlDocument.DocumentNode.SelectNodes(itemPath);

                if (itemNodes != null)
                {
                    foreach (var item in itemNodes)
                    {
                        MangaInfo cmi = new MangaInfo();
                        cmi.Urls = new List<string>();
                        cmi.Cc = cc;
                        cmi.MangeUrl = "http://www.hanhande.net/manga/" + item.Attributes["data-key"].Value.Trim();
                        var nameNode = item.ChildNodes.FindFirst("a");

                        if (nameNode != null)
                        {
                            cmi.MangaName = FileUtility.ReplaceInvalidChar(nameNode.Attributes["title"].Value.Trim());

                            var picNode = nameNode.ChildNodes.FindFirst("img");

                            if (picNode != null)
                            {
                                cmi.MangaPic = picNode.Attributes["src"].Value.Trim();

                                ret.Add(cmi);
                            }
                        }
                    }
                }
            }

            return ret;
        }

        private List<MangaInfo> SearchWuwu(string content)
        {
            List<MangaInfo> ret = new List<MangaInfo>();
            CookieContainer cc = new CookieContainer();
            cc.Add(HtmlManager.GetCookies(si.SourceUrl));

            rcbLog.AppendText("开始搜索污污漫画" + Environment.NewLine);

            var searchUrl = si.SourceSearch;
            content = HttpUtility.UrlEncode(content, Encoding.UTF8);

            var htmlRet = HtmlManager.GetHtmlWebClient(si.SourceHost, searchUrl + content, cc);

            //暂时只搜第一页
            if (htmlRet.Success)
            {
                HtmlAgilityPack.HtmlDocument htmlDocument = new HtmlAgilityPack.HtmlDocument();
                htmlDocument.LoadHtml(htmlRet.Content);

                var itemPath = "//li[@class='item']";

                var itemNodes = htmlDocument.DocumentNode.SelectNodes(itemPath);

                if (itemNodes != null)
                {
                    foreach (var item in itemNodes)
                    {
                        MangaInfo cmi = new MangaInfo();
                        cmi.Urls = new List<string>();
                        cmi.Cc = cc;
                        var nameNode = item.ChildNodes.FindFirst("a");

                        if (nameNode != null)
                        {
                            cmi.MangeUrl = nameNode.Attributes["href"].Value.Trim();
                            cmi.MangaName = FileUtility.ReplaceInvalidChar(nameNode.Attributes["title"].Value.Trim());

                            var picNode = nameNode.ChildNodes.FindFirst("img");

                            if (picNode != null)
                            {
                                cmi.MangaPic = picNode.Attributes["data-src"].Value.Trim();

                                ret.Add(cmi);
                            }
                        }
                    }
                }
            }

            return ret;
        }

        private List<MangaInfo> SearchZhiyinmanke(string content)
        {
            List<MangaInfo> ret = new List<MangaInfo>();
            CookieContainer cc = new CookieContainer();
            cc.Add(HtmlManager.GetCookies(si.SourceUrl));

            rcbLog.AppendText("开始搜索知音漫客" + Environment.NewLine);

            var searchUrl = si.SourceSearch;
            content = HttpUtility.UrlEncode(content, Encoding.UTF8);

            var htmlRet = HtmlManager.GetHtmlWebClient(si.SourceHost, searchUrl + content, cc);

            //暂时只搜第一页
            if (htmlRet.Success)
            {
                HtmlAgilityPack.HtmlDocument htmlDocument = new HtmlAgilityPack.HtmlDocument();
                htmlDocument.LoadHtml(htmlRet.Content);

                var itemPath = "//ul[@class='comic-list col5 clearfix']//li[@class='item']";

                var itemNodes = htmlDocument.DocumentNode.SelectNodes(itemPath);

                if (itemNodes != null)
                {
                    foreach (var item in itemNodes)
                    {
                        MangaInfo cmi = new MangaInfo();
                        cmi.Urls = new List<string>();
                        cmi.Cc = cc;
                        var nameNode = item.ChildNodes.FindFirst("a");

                        if (nameNode != null)
                        {
                            cmi.MangeUrl = "http://www.zuozuomanhua.com" + nameNode.Attributes["href"].Value.Trim();
                            cmi.MangaName = FileUtility.ReplaceInvalidChar(nameNode.Attributes["title"].Value.Trim());

                            var picNode = nameNode.ChildNodes.FindFirst("img");

                            if (picNode != null)
                            {
                                cmi.MangaPic = picNode.Attributes["data-src"].Value.Trim();

                                ret.Add(cmi);
                            }
                        }
                    }
                }
            }

            return ret;
        }

        private List<MangaInfo> SearchManhuadb(string content)
        {
            List<MangaInfo> ret = new List<MangaInfo>();
            CookieContainer cc = new CookieContainer();
            cc.Add(HtmlManager.GetCookies(si.SourceUrl));

            rcbLog.AppendText("开始搜索漫画DB" + Environment.NewLine);

            var searchUrl = si.SourceSearch;
            content = HttpUtility.UrlEncode(content, Encoding.UTF8);

            var htmlRet = HtmlManager.GetHtmlWebClient(si.SourceHost, searchUrl + content, cc);

            //暂时只搜第一页
            if (htmlRet.Success)
            {
                HtmlAgilityPack.HtmlDocument htmlDocument = new HtmlAgilityPack.HtmlDocument();
                htmlDocument.LoadHtml(htmlRet.Content);

                var itemPath = "//div[@class='comicbook-index mb-2 mb-sm-0']";

                var itemNodes = htmlDocument.DocumentNode.SelectNodes(itemPath);

                if (itemNodes != null)
                {
                    foreach (var item in itemNodes)
                    {
                        MangaInfo cmi = new MangaInfo();
                        cmi.Urls = new List<string>();
                        cmi.Cc = cc;
                        var nameNode = item.ChildNodes.FindFirst("a");

                        if (nameNode != null)
                        {
                            cmi.MangeUrl = "https://www.manhuadb.com" + nameNode.Attributes["href"].Value.Trim();
                            cmi.MangaName = FileUtility.ReplaceInvalidChar(nameNode.Attributes["title"].Value.Trim());

                            var picNode = nameNode.ChildNodes.FindFirst("img");

                            if (picNode != null)
                            {
                                cmi.MangaPic = picNode.Attributes["src"].Value.Trim();

                                ret.Add(cmi);
                            }
                        }
                    }
                }
            }

            return ret;
        }

        private void AddAdditionalInfoWuwu()
        {
            var setting = SettingSevice.ReadSetting();

            if (string.IsNullOrEmpty(setting.HistoryFolder) || string.IsNullOrEmpty(setting.ZipFolder) || string.IsNullOrEmpty(setting.MangaFolder))
            {
                MessageBox.Show("请先去设置页面完成设置");
                return;
            }

            var history = HistoryService.ReadHistory(si.SourceName, mi.MangaName, setting.HistoryFolder);

            var htmlRet = HtmlManager.GetHtmlWebClient(si.SourceHost, mi.MangeUrl, mi.Cc);

            if (htmlRet.Success)
            {
                HtmlAgilityPack.HtmlDocument htmlDocument = new HtmlAgilityPack.HtmlDocument();
                htmlDocument.LoadHtml(htmlRet.Content);
                var chapterPath = "//ul[@class='chapter-list clearfix']//li";
                var updatePath = "//p[@class='update']";

                var chapterNodes = htmlDocument.DocumentNode.SelectNodes(chapterPath);
                var updateNode = htmlDocument.DocumentNode.SelectSingleNode(updatePath);

                if (chapterNodes != null)
                {
                    lvwMainList.BeginUpdate();
                    int count = 1;
                    foreach (var node in chapterNodes)
                    {
                        var aTag = node.ChildNodes.FindFirst("a");

                        if (aTag != null)
                        {
                            var subUrl = aTag.Attributes["href"].Value.Trim();
                            var title = aTag.InnerHtml.Trim();

                            mi.Urls.Add(subUrl);

                            ListViewItem lvi = new ListViewItem(title);
                            lvi.Tag = subUrl;

                            if (count % 2 == 0)
                            {
                                lvi.BackColor = Color.LightGray;
                            }

                            if (!history.DownloadedChapters.Contains(subUrl))
                            {
                                lvi.Selected = true;
                            }

                            lvwMainList.Items.Add(lvi);
                            count++;
                        }
                    }

                    mi.MangaChapters = count;
                    lvwMainList.EndUpdate();

                    txtMainChapters.Text = count + "章";
                    txtMainStatus.Text = updateNode.InnerHtml.Trim();
                    txtMainLastChapter.Text = history.DownloadedChapters.Count + "章";
                }
            }
        }

        private void AddAdditionalInfoManhuadb()
        {
            var setting = SettingSevice.ReadSetting();

            if (string.IsNullOrEmpty(setting.HistoryFolder) || string.IsNullOrEmpty(setting.ZipFolder) || string.IsNullOrEmpty(setting.MangaFolder))
            {
                MessageBox.Show("请先去设置页面完成设置");
                return;
            }

            var history = HistoryService.ReadHistory(si.SourceName, mi.MangaName, setting.HistoryFolder);

            var htmlRet = HtmlManager.GetHtmlWebClient(si.SourceHost, mi.MangeUrl, mi.Cc);

            if (htmlRet.Success)
            {
                HtmlAgilityPack.HtmlDocument htmlDocument = new HtmlAgilityPack.HtmlDocument();
                htmlDocument.LoadHtml(htmlRet.Content);
                var chapterPath = "//div[@id='comic-book-list']//div[1]//ol//li";

                var chapterNodes = htmlDocument.DocumentNode.SelectNodes(chapterPath);

                if (chapterNodes != null)
                {
                    lvwMainList.BeginUpdate();
                    int count = 1;
                    foreach (var node in chapterNodes)
                    {
                        var aTag = node.ChildNodes.FindFirst("a");

                        if (aTag != null)
                        {
                            var subUrl = aTag.Attributes["href"].Value.Trim();
                            var title = aTag.InnerHtml.Trim();

                            mi.Urls.Add("https://www.manhuadb.com/" + subUrl);

                            ListViewItem lvi = new ListViewItem(title);
                            lvi.Tag = "https://www.manhuadb.com/" + subUrl;

                            if (count % 2 == 0)
                            {
                                lvi.BackColor = Color.LightGray;
                            }

                            if (!history.DownloadedChapters.Contains(subUrl))
                            {
                                lvi.Selected = true;
                            }

                            lvwMainList.Items.Add(lvi);
                            count++;
                        }
                    }

                    mi.MangaChapters = count;
                    lvwMainList.EndUpdate();

                    txtMainChapters.Text = count + "章";
                    txtMainLastChapter.Text = history.DownloadedChapters.Count + "章";
                }
            }
        }

        private void AddAdditionalInfoZhiyinmanke()
        {
            var setting = SettingSevice.ReadSetting();

            if (string.IsNullOrEmpty(setting.HistoryFolder) || string.IsNullOrEmpty(setting.ZipFolder) || string.IsNullOrEmpty(setting.MangaFolder))
            {
                MessageBox.Show("请先去设置页面完成设置");
                return;
            }

            var history = HistoryService.ReadHistory(si.SourceName, mi.MangaName, setting.HistoryFolder);

            var htmlRet = HtmlManager.GetHtmlWebClient(si.SourceHost, mi.MangeUrl, mi.Cc);

            if (htmlRet.Success)
            {
                HtmlAgilityPack.HtmlDocument htmlDocument = new HtmlAgilityPack.HtmlDocument();
                htmlDocument.LoadHtml(htmlRet.Content);
                var chapterPath = "//ul[@class='chapter-list clearfix']//li";
                var updatePath = "//p[@class='update']";

                var chapterNodes = htmlDocument.DocumentNode.SelectNodes(chapterPath);
                var updateNode = htmlDocument.DocumentNode.SelectSingleNode(updatePath);

                if (chapterNodes != null)
                {
                    lvwMainList.BeginUpdate();
                    int count = 1;
                    foreach (var node in chapterNodes)
                    {
                        var aTag = node.ChildNodes.FindFirst("a");

                        if (aTag != null)
                        {
                            var subUrl = aTag.Attributes["href"].Value.Trim();
                            var title = aTag.InnerHtml.Trim();

                            mi.Urls.Add("http://www.zuozuomanhua.com/" + subUrl);

                            ListViewItem lvi = new ListViewItem(title);
                            lvi.Tag = "http://www.zuozuomanhua.com/" + subUrl;

                            if (count % 2 == 0)
                            {
                                lvi.BackColor = Color.LightGray;
                            }

                            if (!history.DownloadedChapters.Contains(subUrl))
                            {
                                lvi.Selected = true;
                            }

                            lvwMainList.Items.Add(lvi);
                            count++;
                        }
                    }

                    mi.MangaChapters = count;
                    lvwMainList.EndUpdate();

                    txtMainChapters.Text = count + "章";
                    txtMainStatus.Text = updateNode.InnerHtml.Trim();
                    txtMainLastChapter.Text = history.DownloadedChapters.Count + "章";
                }
            }
        }

        private void AddAddtionalInfoHanhan()
        {
            var setting = SettingSevice.ReadSetting();

            if (string.IsNullOrEmpty(setting.HistoryFolder) || string.IsNullOrEmpty(setting.ZipFolder) || string.IsNullOrEmpty(setting.MangaFolder))
            {
                MessageBox.Show("请先去设置页面完成设置");
                return;
            }

            var history = HistoryService.ReadHistory(si.SourceName, mi.MangaName, setting.HistoryFolder);

            var htmlRet = HtmlManager.GetHtmlWebClient(si.SourceHost, mi.MangeUrl, mi.Cc);

            if (htmlRet.Success)
            {
                HtmlAgilityPack.HtmlDocument htmlDocument = new HtmlAgilityPack.HtmlDocument();
                htmlDocument.LoadHtml(htmlRet.Content);
                var statusPath = "//span[@class='serial']";
                var chapterPath = "//ul[@id='chapter-list-4']//li";
                var updatePath = "//li[@class='status']//span[@class='red']";

                var chapterNodes = htmlDocument.DocumentNode.SelectNodes(chapterPath);
                var statusNode = htmlDocument.DocumentNode.SelectSingleNode(statusPath);
                var updateNode = htmlDocument.DocumentNode.SelectSingleNode(updatePath);

                if (chapterNodes != null)
                {
                    lvwMainList.BeginUpdate();
                    int count = 1;
                    foreach (var node in chapterNodes)
                    {
                        var aTag = node.ChildNodes.FindFirst("a");
                        var sTag = node.ChildNodes.FindFirst("span");
                        if (aTag != null && sTag != null)
                        {
                            var subUrl = "http://www.hanhande.net" + aTag.Attributes["href"].Value.Trim();
                            mi.Urls.Add(subUrl);

                            ListViewItem lvi = new ListViewItem(sTag.InnerHtml.Trim());
                            lvi.Tag = subUrl;

                            if (count % 2 == 0)
                            {
                                lvi.BackColor = Color.LightGray;
                            }

                            if (!history.DownloadedChapters.Contains(subUrl))
                            {
                                lvi.Selected = true;
                            }

                            lvwMainList.Items.Add(lvi);
                            count++;
                        }
                    }

                    mi.MangaChapters = count;
                    lvwMainList.EndUpdate();

                    txtMainChapters.Text = count + "章";
                    var statusStr = statusNode == null ? "已完结 " : "连载中 ";
                    statusStr += updateNode == null ? "无更新" : "更新于 " + updateNode.InnerHtml.Trim();
                    txtMainStatus.Text = statusStr;
                    txtMainLastChapter.Text = history.DownloadedChapters.Count + "章";
                }
            }
        }

        private void AddAddtionalInfoHmb()
        {
            var setting = SettingSevice.ReadSetting();

            if (string.IsNullOrEmpty(setting.HistoryFolder) || string.IsNullOrEmpty(setting.ZipFolder) || string.IsNullOrEmpty(setting.MangaFolder))
            {
                MessageBox.Show("请先去设置页面完成设置");
                return;
            }

            var history = HistoryService.ReadHistory(si.SourceName, mi.MangaName, setting.HistoryFolder);

            var htmlRet = HtmlManager.GetHtmlWebClient(si.SourceHost, mi.MangeUrl, mi.Cc);

            if (htmlRet.Success)
            {
                int index = 0;
                string updateDate = "";
                HtmlAgilityPack.HtmlDocument htmlDocument = new HtmlAgilityPack.HtmlDocument();
                htmlDocument.LoadHtml(htmlRet.Content);
                string infoPath = "//div[@id='info']";
                string statusPath = "//span[@class='b']";
                string chapterPath = "//dd";

                var statusNode = htmlDocument.DocumentNode.SelectSingleNode(statusPath);
                var chapterNodes = htmlDocument.DocumentNode.SelectNodes(chapterPath);
                var infoNode = htmlDocument.DocumentNode.SelectSingleNode(infoPath);

                if (infoNode != null && infoNode.ChildNodes.Count >= 8)
                {
                    updateDate = infoNode.ChildNodes[7].InnerText.Trim();
                }

                lvwMainList.BeginUpdate();

                foreach (var chapter in chapterNodes)
                {
                    if (chapter.ParentNode != null && chapter.ParentNode.Name == "dl" && chapter.ChildNodes.FindFirst("a") != null)
                    {
                        var subUrl = si.SourceUrl + chapter.ChildNodes.FindFirst("a").Attributes["href"].Value.Trim();
                        var subTitle = chapter.ChildNodes.FindFirst("a").InnerText.Trim();

                        ListViewItem lvi = new ListViewItem(subTitle);
                        lvi.Tag = subUrl;

                        if (index % 2 == 0)
                        {
                            lvi.BackColor = Color.LightGray;
                        }

                        if (!history.DownloadedChapters.Contains(subUrl))
                        {
                            lvi.Selected = true;
                        }

                        lvwMainList.Items.Add(lvi);

                        mi.Urls.Add(subUrl);

                        index++;
                    }
                }

                lvwMainList.EndUpdate();

                txtMainChapters.Text = index + "章";
                var statusStr = statusNode == null ? "已完结 " : "连载中 ";
                statusStr += updateDate;
                txtMainStatus.Text = statusStr;
                txtMainLastChapter.Text = history.DownloadedChapters.Count + "章";
            }
        }

        private void ShowMangeWuwu()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(mi.MangaPic);
            request.Host = "pic.muamh.com";

            try
            {
                picManga.Image = Image.FromStream(request.GetResponse().GetResponseStream());
            }
            catch (Exception ee)
            {

            }

            txtMainUrl.Text = mi.MangeUrl;

            lvwMainList.Focus();
        }

        private void ShowMangeManhuadb()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(mi.MangaPic);
            request.Host = "media.manhuadb.com";

            try
            {
                picManga.Image = Image.FromStream(request.GetResponse().GetResponseStream());
            }
            catch (Exception ee)
            {

            }

            txtMainUrl.Text = mi.MangeUrl;

            lvwMainList.Focus();
        }

        private void ShowMangeZhiyinmanke()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(mi.MangaPic);
            request.Host = "tu.jiayouzhibo.com";

            try
            {
                picManga.Image = Image.FromStream(request.GetResponse().GetResponseStream());
            }
            catch (Exception ee)
            {

            }

            txtMainUrl.Text = mi.MangeUrl;

            lvwMainList.Focus();
        }

        private void ShowMangeHanhan()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(mi.MangaPic);
            request.Host = "i.jituoli.com";

            if (mi.MangaPic.Contains("shaque.vip"))
            {
                request.Host = "img001.shaque.vip";
            }

            request.Referer = "http://www.hanhande.net/search/?keywords=";

            try
            {
                picManga.Image = Image.FromStream(request.GetResponse().GetResponseStream());
            }
            catch (Exception ee)
            {

            }

            txtMainUrl.Text = mi.MangeUrl;

            lvwMainList.Focus();
        }

        private void ShowMangeHmb()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(mi.MangaPic);
            request.Host = "img.manjiavip.com";
            request.Referer = "http://www.hmba.vip/home/search";

            picManga.Image = Image.FromStream(request.GetResponse().GetResponseStream());

            txtMainUrl.Text = mi.MangeUrl;

            lvwMainList.Focus();
        }

        private void CombinePics(string root, string title, string sub)
        {
            if (rbCombine.Checked)
            {
                UpdateRtb(rcbLog, "合并图片");

                var totalPics = Directory.GetFiles(sub).ToList();
                int index = 1;

                while (totalPics.Count > 0)
                {
                    int height = 0;
                    int take = 0;

                    for (int i = 0; i < totalPics.Count; i++)
                    {
                        Image tempImg = Image.FromFile(totalPics[i]);
   
                        if (height + tempImg.Height < 63000)
                        {
                            height += tempImg.Height;
                            take++;

                            tempImg.Dispose();
                        }
                        else
                        {
                            tempImg.Dispose();
                            break;
                        }
                    }

                    var batchPics = totalPics.Take(take).ToList();
                    ImageHelper.CombinePics(sub + title + "_" + index + ".jpg", batchPics, sub, false);
                    totalPics = totalPics.Skip(take).ToList();

                    index++;
                }
            }
        }

        private string DownloadHmb(Dictionary<string, string> dic, SettingModel setting)
        {
            var mangaRoot = GenerateFolder(setting.MangaFolder + si.SourceName + "_" + mi.MangaName + "\\");

            Dictionary<string, string> downloadUrls = dic;

            var history = HistoryService.ReadHistory(si.SourceName, mi.MangaName, setting.HistoryFolder);

            int currentChapert = 1;

            foreach (var url in downloadUrls)
            {
                DialogResult rs = DialogResult.No;

                if (history.DownloadedChapters.Contains(url.Key))
                {
                    rs = MessageBox.Show("之前已经下载过该章节是否要跳过", "提示", MessageBoxButtons.YesNo);
                }

                if (rs == DialogResult.Yes)
                {
                    UpdateRtb(rcbLog, url.Value + " 已经下载过,跳过");
                    continue;
                }
                else
                {
                    var htmlRet = HtmlManager.GetHtmlWebClient(si.SourceHost, url.Key, mi.Cc);

                    if (htmlRet.Success)
                    {
                        if (!htmlRet.Content.Contains("文章不存在"))
                        {
                            var downloadExcep = "";

                            var subFolder = GenerateFolder(mangaRoot + FileUtility.ReplaceInvalidChar(url.Value) + "\\");

                            HtmlAgilityPack.HtmlDocument htmlDocument = new HtmlAgilityPack.HtmlDocument();
                            htmlDocument.LoadHtml(htmlRet.Content);
                            string picPath = "//img";
                            var picNodes = htmlDocument.DocumentNode.SelectNodes(picPath);

                            int retry = 1;

                            if (picNodes == null)
                            {
                                while (retry <= 3)
                                {
                                    UpdateRtb(rcbLog, "重试 " + retry + " 次" + Environment.NewLine);

                                    htmlRet = HtmlManager.GetHtmlWebClient(si.SourceHost, url.Key, mi.Cc);
                                    if (htmlRet.Success)
                                    {
                                        htmlDocument = new HtmlAgilityPack.HtmlDocument();
                                        htmlDocument.LoadHtml(htmlRet.Content);
                                        picNodes = htmlDocument.DocumentNode.SelectNodes(picPath);
                                    }

                                    retry++;
                                }
                            }

                            if (picNodes != null)
                            {
                                Dictionary<int, string> picToBeDownloaded = new Dictionary<int, string>();
                                int index = 1;

                                foreach (var p in picNodes)
                                {
                                    if (p.Attributes != null && p.Attributes["src"] != null && !string.IsNullOrEmpty(p.Attributes["src"].Value))
                                    {
                                        picToBeDownloaded.Add(index, p.Attributes["src"].Value);
                                        index++;
                                    }
                                }

                                int finish = 0;

                                Parallel.ForEach(picToBeDownloaded, new ParallelOptions { MaxDegreeOfParallelism = setting.ThreadCount }, node =>
                                {
                                    downloadExcep = "";

                                    downloadExcep += DownloadHelper.DownloadFile(node.Value, subFolder + node.Key + ".jpg", "img.manjiavip.com", url.Key);

                                    UpdatePb(pbSub, ++finish, picToBeDownloaded.Count);
                                });


                                CombinePics(mangaRoot, url.Value, subFolder);
                            }
                            else
                            {
                                UpdateRtb(rcbLog, "图片获取失败" + Environment.NewLine);
                            }

                            if (string.IsNullOrEmpty(downloadExcep))
                            {
                                HistoryService.WriteHistory(si.SourceName, mi.MangaName, setting.HistoryFolder, url.Key, mi.MangeUrl);
                                UpdateRtb(rcbLog, "下载" + url.Value + " 成功");
                            }
                            else
                            {
                                UpdateRtb(rcbLog, "下载" + url.Value + " 部分失败");
                            }
                        }
                        else
                        {
                            UpdateRtb(rcbLog, url.Value + "不存在" + Environment.NewLine);
                        }
                    }
                }

                UpdatePb(pbMain, currentChapert, downloadUrls.Count);
                currentChapert++;
            }

            return mangaRoot;
        }

        private string DownloadHanhan(Dictionary<string, string> dic, SettingModel setting)
        {
            var mangaRoot = GenerateFolder(setting.MangaFolder + si.SourceName + "_" + mi.MangaName + "\\");

            Dictionary<string, string> downloadUrls = dic;

            var history = HistoryService.ReadHistory(si.SourceName, mi.MangaName, setting.HistoryFolder);

            int currentChapert = 1;

            foreach (var url in downloadUrls)
            {
                DialogResult rs = DialogResult.No;

                if (history.DownloadedChapters.Contains(url.Key))
                {
                    rs = MessageBox.Show("之前已经下载过该章节是否要跳过", "提示", MessageBoxButtons.YesNo);
                }

                if (rs == DialogResult.Yes)
                {
                    UpdateRtb(rcbLog, url.Value + " 已经下载过,跳过");
                    continue;
                }
                else
                {
                    var htmlRet = HtmlManager.GetHtmlWebClient(si.SourceHost, url.Key, mi.Cc);

                    if (htmlRet.Success)
                    {
                        var downloadExcep = "";
                        var subFolder = GenerateFolder(mangaRoot + FileUtility.ReplaceInvalidChar(url.Value) + "\\");

                        HtmlAgilityPack.HtmlDocument htmlDocument = new HtmlAgilityPack.HtmlDocument();
                        htmlDocument.LoadHtml(htmlRet.Content);

                        var urlPath = "//link[@rel='miphtml']";

                        var urlNode = htmlDocument.DocumentNode.SelectSingleNode(urlPath);

                        var decrypt = htmlRet.Content.Substring(htmlRet.Content.IndexOf("chapterImages = ") + "chapterImages = ".Length);
                        decrypt = decrypt.Substring(0, decrypt.IndexOf("]") + 1);

                        var urlHeader = htmlRet.Content.Substring(htmlRet.Content.IndexOf("chapterPath = \"") + "chapterPath = \"".Length);
                        urlHeader = urlHeader.Substring(0, urlHeader.IndexOf("\""));

                        var picList = JsonConvert.DeserializeObject<List<string>>(decrypt);

                        if (picList != null && picList.Count > 0)
                        {
                            Dictionary<int, string> picToBeDownloaded = new Dictionary<int, string>();
                            int index = 1;
                            foreach (var p in picList)
                            {
                                picToBeDownloaded.Add(index, p);
                                index++;
                            }

                            int finish = 0;

                            Parallel.ForEach(picToBeDownloaded, new ParallelOptions { MaxDegreeOfParallelism = setting.ThreadCount }, node =>
                            {
                                downloadExcep = "";
                                string host = "res.img.jituoli.com";
                                var pic = node.Value;
                                bool shaque = false;

                                if (!node.Value.StartsWith("http"))
                                {
                                    host = "img001.shaque.vip";
                                    pic = "http://img001.shaque.vip/" + urlHeader + node.Value;
                                    shaque = true;
                                }
   
                                downloadExcep += DownloadHelper.DownloadHttpsWithHost(pic, subFolder + node.Key + ".jpg", host, urlNode.Attributes["href"].Value.Trim(), shaque);

                                UpdatePb(pbSub, ++finish, picToBeDownloaded.Count);
                            });


                            CombinePics(mangaRoot, url.Value, subFolder);
                        }
                        else
                        {
                            UpdateRtb(rcbLog, "图片获取失败" + Environment.NewLine);
                        }

                        if (string.IsNullOrEmpty(downloadExcep))
                        {
                            HistoryService.WriteHistory(si.SourceName, mi.MangaName, setting.HistoryFolder, url.Key, mi.MangeUrl);
                            UpdateRtb(rcbLog, "下载" + url.Value + " 成功");
                        }
                        else
                        {
                            UpdateRtb(rcbLog, "下载" + url.Value + " 部分失败");
                        }

                        UpdatePb(pbMain, currentChapert, downloadUrls.Count);
                        currentChapert++;
                    }
                }
            }

            return mangaRoot;
        }

        private string DownloadWuwu(Dictionary<string, string> dic, SettingModel setting)
        {
            var mangaRoot = GenerateFolder(setting.MangaFolder + si.SourceName + "_" + mi.MangaName + "\\");

            Dictionary<string, string> downloadUrls = dic;

            var history = HistoryService.ReadHistory(si.SourceName, mi.MangaName, setting.HistoryFolder);

            int currentChapert = 1;

            foreach (var url in downloadUrls)
            {
                DialogResult rs = DialogResult.No;

                if (history.DownloadedChapters.Contains(url.Key))
                {
                    rs = MessageBox.Show("之前已经下载过该章节是否要跳过", "提示", MessageBoxButtons.YesNo);
                }

                if (rs == DialogResult.Yes)
                {
                    UpdateRtb(rcbLog, url.Value + " 已经下载过,跳过");
                    continue;
                }
                else
                {
                    var htmlRet = HtmlManager.GetHtmlWebClient(si.SourceHost, url.Key, mi.Cc);

                    if (htmlRet.Success)
                    {
                        var downloadExcep = "";
                        var subFolder = GenerateFolder(mangaRoot + FileUtility.ReplaceInvalidChar(url.Value) + "\\");

                        HtmlAgilityPack.HtmlDocument htmlDocument = new HtmlAgilityPack.HtmlDocument();
                        htmlDocument.LoadHtml(htmlRet.Content);

                        var urlPath = "//div[@class='comiclist']//div[@class='comicpage']//img";

                        var urlNode = htmlDocument.DocumentNode.SelectNodes(urlPath);

                        if (urlNode != null && urlNode.Count > 0)
                        {
                            Dictionary<int, string> picToBeDownloaded = new Dictionary<int, string>();
                            int index = 1;
                            foreach (var p in urlNode)
                            {
                                picToBeDownloaded.Add(index, p.Attributes["src"].Value.Trim());
                                index++;
                            }

                            int finish = 0;

                            Parallel.ForEach(picToBeDownloaded, new ParallelOptions { MaxDegreeOfParallelism = setting.ThreadCount }, node =>
                            {
                                downloadExcep = "";
                                string host = "pic.muamh.com";
                                var pic = node.Value;

                                downloadExcep += DownloadHelper.DownloadHttpsWithHost(pic, subFolder + node.Key + ".jpg", host, "", false);

                                UpdatePb(pbSub, ++finish, picToBeDownloaded.Count);
                            });

                            CombinePics(mangaRoot, url.Value, subFolder);
                        }
                        else
                        {
                            UpdateRtb(rcbLog, "图片获取失败" + Environment.NewLine);
                        }

                        if (string.IsNullOrEmpty(downloadExcep))
                        {
                            HistoryService.WriteHistory(si.SourceName, mi.MangaName, setting.HistoryFolder, url.Key, mi.MangeUrl);
                            UpdateRtb(rcbLog, "下载" + url.Value + " 成功");
                        }
                        else
                        {
                            UpdateRtb(rcbLog, "下载" + url.Value + " 部分失败");
                        }

                        UpdatePb(pbMain, currentChapert, downloadUrls.Count);
                        currentChapert++;
                    }
                }
            }

            return mangaRoot;
        }

        private string DownloadZhiyinmanke(Dictionary<string, string> dic, SettingModel setting)
        {
            var mangaRoot = GenerateFolder(setting.MangaFolder + si.SourceName + "_" + mi.MangaName.Replace(".","") + "\\");

            Dictionary<string, string> downloadUrls = dic;

            var history = HistoryService.ReadHistory(si.SourceName, mi.MangaName, setting.HistoryFolder);

            int currentChapert = 1;

            foreach (var url in downloadUrls)
            {
                DialogResult rs = DialogResult.No;

                if (history.DownloadedChapters.Contains(url.Key))
                {
                    rs = MessageBox.Show("之前已经下载过该章节是否要跳过", "提示", MessageBoxButtons.YesNo);
                }

                if (rs == DialogResult.Yes)
                {
                    UpdateRtb(rcbLog, url.Value + " 已经下载过,跳过");
                    continue;
                }
                else
                {
                    var htmlRet = HtmlManager.GetHtmlWebClient(si.SourceHost, url.Key, mi.Cc);

                    if (htmlRet.Success)
                    {
                        var downloadExcep = "";
                        var subFolder = GenerateFolder(mangaRoot + FileUtility.ReplaceInvalidChar(url.Value) + "\\");

                        HtmlAgilityPack.HtmlDocument htmlDocument = new HtmlAgilityPack.HtmlDocument();
                        htmlDocument.LoadHtml(htmlRet.Content);

                        var urlPath = "//div[@class='comiclist']//div[@class='comicpage']//img";

                        var urlNode = htmlDocument.DocumentNode.SelectNodes(urlPath);

                        if (urlNode != null && urlNode.Count > 0)
                        {
                            Dictionary<int, string> picToBeDownloaded = new Dictionary<int, string>();
                            int index = 1;
                            foreach (var p in urlNode)
                            {
                                picToBeDownloaded.Add(index, p.Attributes["src"].Value.Trim());
                                index++;
                            }

                            int finish = 0;

                            Parallel.ForEach(picToBeDownloaded, new ParallelOptions { MaxDegreeOfParallelism = setting.ThreadCount }, node =>
                            {
                                downloadExcep = "";
                                string host = "tu.jiayouzhibo.com";
                                var pic = node.Value;

                                downloadExcep += DownloadHelper.DownloadHttpsWithHost(pic, subFolder + node.Key + ".jpg", host, "", false);

                                UpdatePb(pbSub, ++finish, picToBeDownloaded.Count);
                            });

                            CombinePics(mangaRoot, url.Value, subFolder);
                        }
                        else
                        {
                            UpdateRtb(rcbLog, "图片获取失败" + Environment.NewLine);
                        }

                        if (string.IsNullOrEmpty(downloadExcep))
                        {
                            HistoryService.WriteHistory(si.SourceName, mi.MangaName, setting.HistoryFolder, url.Key, mi.MangeUrl);
                            UpdateRtb(rcbLog, "下载" + url.Value + " 成功");
                        }
                        else
                        {
                            UpdateRtb(rcbLog, "下载" + url.Value + " 部分有重试下载，可能有部分失败");
                        }

                        UpdatePb(pbMain, currentChapert, downloadUrls.Count);
                        currentChapert++;
                    }
                }
            }

            return mangaRoot;
        }

        private string DownloadManhuadb(Dictionary<string, string> dic, SettingModel setting)
        {
            var mangaRoot = GenerateFolder(setting.MangaFolder + si.SourceName + "_" + mi.MangaName.Replace(".", "") + "\\");

            Dictionary<string, string> downloadUrls = dic;

            var history = HistoryService.ReadHistory(si.SourceName, mi.MangaName, setting.HistoryFolder);

            int currentChapert = 1;

            foreach (var url in downloadUrls)
            {
                DialogResult rs = DialogResult.No;

                if (history.DownloadedChapters.Contains(url.Key))
                {
                    rs = MessageBox.Show("之前已经下载过该章节是否要跳过", "提示", MessageBoxButtons.YesNo);
                }

                if (rs == DialogResult.Yes)
                {
                    UpdateRtb(rcbLog, url.Value + " 已经下载过,跳过");
                    continue;
                }
                else
                {
                    var htmlRet = HtmlManager.GetHtmlWebClient(si.SourceHost, url.Key, mi.Cc);

                    if (htmlRet.Success)
                    {
                        var downloadExcep = "";
                        var subFolder = GenerateFolder(mangaRoot + FileUtility.ReplaceInvalidChar(url.Value) + "\\");

                        HtmlAgilityPack.HtmlDocument htmlDocument = new HtmlAgilityPack.HtmlDocument();
                        htmlDocument.LoadHtml(htmlRet.Content);

                        var urlPath = "//div[@class='d-none vg-r-data']";

                        var urlNode = htmlDocument.DocumentNode.SelectSingleNode(urlPath);

                        var picPrexix = "";
                        var base64Str = htmlRet.Content.Substring(htmlRet.Content.IndexOf("var img_data = '") + "var img_data = '".Length);
                        base64Str = base64Str.Substring(0, base64Str.IndexOf("'"));
                        var picUrl = DecryptHelper.Base64Decode(base64Str);
                        var pics = JsonConvert.DeserializeObject<List<ManhuadbPic>>(picUrl);

                        if (urlNode != null)
                        {
                            picPrexix = urlNode.Attributes["data-host"].Value + urlNode.Attributes["data-img_pre"].Value;
                        }

                        Dictionary<int, string> picToBeDownloaded = new Dictionary<int, string>();

                        foreach (var p in pics)
                        {
                            picToBeDownloaded.Add(p.p, picPrexix + p.img);
                        }

                        int finish = 0;

                        Parallel.ForEach(picToBeDownloaded, new ParallelOptions { MaxDegreeOfParallelism = setting.ThreadCount }, node =>
                        {
                            downloadExcep = "";
                            string host = "i1.manhuadb.com";
                            var pic = node.Value;

                            downloadExcep += DownloadHelper.DownloadHttpsWithHost(pic, subFolder + node.Key + ".jpg", host, "", false);

                            UpdatePb(pbSub, ++finish, picToBeDownloaded.Count);
                        });

                        CombinePics(mangaRoot, url.Value, subFolder);

                        if (string.IsNullOrEmpty(downloadExcep))
                        {
                            HistoryService.WriteHistory(si.SourceName, mi.MangaName, setting.HistoryFolder, url.Key, mi.MangeUrl);
                            UpdateRtb(rcbLog, "下载" + url.Value + " 成功");
                        }
                        else
                        {
                            UpdateRtb(rcbLog, "下载" + url.Value + " 部分有重试下载，可能有部分失败");
                        }

                        UpdatePb(pbMain, currentChapert, downloadUrls.Count);
                        currentChapert++;
                    }
                }
            }

            return mangaRoot;
        }

        private string GenerateFolder(string path)
        {
            string ret = path;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return ret;
        }

        #endregion

        #endregion
    }
}
