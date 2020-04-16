using DataBaseManager.JavDataBaseHelper;
using Model.JavModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utils;

namespace AvReName
{
    public partial class Fetch : Form
    {
        private static string imgFolder = JavINIClass.IniReadValue("Jav", "imgFolder");
        private static string detailTitlePattern = JavINIClass.IniReadValue("Jav", "detailTitle");
        private static string detailImgPattern = JavINIClass.IniReadValue("Jav", "detailImg");
        private static string detailIDPattern = JavINIClass.IniReadValue("Jav", "detailID");
        private static string detailDatePattern = JavINIClass.IniReadValue("Jav", "detailDate");
        private static string detailLengthPattern = JavINIClass.IniReadValue("Jav", "detailLength");
        private static string detailDirectorPattern = JavINIClass.IniReadValue("Jav", "detailDirector");
        private static string detailCompanyPattern = JavINIClass.IniReadValue("Jav", "detailCompany");
        private static string detailPublisherPattern = JavINIClass.IniReadValue("Jav", "detailPublisher");
        private static string detailCategoryPattern = JavINIClass.IniReadValue("Jav", "detailCategory");
        private static string detailActressPattern = JavINIClass.IniReadValue("Jav", "detailActress");
        private static string detailCommentPattern = JavINIClass.IniReadValue("Jav", "detailComment");

        private AV av = null;

        public Fetch()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            if (av != null)
            {
                if (!JavDataBaseManager.HasAv(av.URL))
                {
                    JavDataBaseManager.InsertAV(av);
                }

                string result = "";
                if (!File.Exists(imgFolder + av.ID + av.Name + ".jpg"))
                {
                    result = DownloadHelper.DownloadFile(av.PictureURL, imgFolder + av.ID + av.Name + ".jpg");
                }

                this.DialogResult = DialogResult.Yes;
                this.Close();
            }
        }

        private void fetchBtn_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(utlText.Text))
            {
                FetchInfo(utlText.Text);
            }

            if (av != null)
            {
                pictureBox1.Image = Image.FromStream(WebRequest.Create(av.PictureURL).GetResponse().GetResponseStream());
                nameLabel.Text = av.Name;
                directorLabel.Text = av.Director;
                publisherLabel.Text = av.Publisher;
                categoryLabel.Text = av.Category;
                dateLabel.Text = av.ReleaseDate.ToString("yyyy-MM-dd");
                companyLabel.Text = av.Company;
            }
            else
            {
                MessageBox.Show("没有找到该AV", "警告");
            }
        }

        private void FetchInfo(string url)
        {
            CookieContainer cc = new CookieContainer();
            var cookieData = new ChromeCookieReader().ReadCookies("javlibrary");

            foreach (var item in cookieData.Where(x => !x.Value.Contains(",")).Distinct())
            {
                Cookie c = new Cookie(item.Name, item.Value, "/", "www.javlibrary.com");

                cc.Add(c);
            }

            cc.Add(new Cookie("over18", "18", "/", "www.javlibrary.com"));

            var ret = HtmlManager.NeedToUpdateCookie(url, "utf-8", true, cc);
            var res = ret.Content;

            if (res.Success)
            {
                av = new AV();

                var m = Regex.Matches(res.Content, detailIDPattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);
                foreach (Match item in m)
                {
                    var data = item.Groups[1].Value;
                    av.ID = data;
                }

                m = Regex.Matches(res.Content, detailTitlePattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);
                foreach (Match item in m)
                {
                    var data = item.Groups[2].Value.Replace(av.ID + " ", "");
                    av.Name = FileUtility.ReplaceInvalidChar(data);
                }

                m = Regex.Matches(res.Content, detailImgPattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);
                foreach (Match item in m)
                {
                    var data = item.Groups[1].Value.StartsWith("http") ? item.Groups[1].Value : "http:" + item.Groups[1].Value;
                    av.PictureURL = data;
                }

                m = Regex.Matches(res.Content, detailDatePattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);
                foreach (Match item in m)
                {
                    var data = item.Groups[1].Value;
                    av.ReleaseDate = DateTime.Parse(data);
                }

                m = Regex.Matches(res.Content, detailLengthPattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);
                foreach (Match item in m)
                {
                    var data = item.Groups[1].Value;
                    av.AvLength = int.Parse(data);
                }

                m = Regex.Matches(res.Content, detailDirectorPattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);
                foreach (Match item in m)
                {
                    var u = item.Groups[1].Value;
                    var data = item.Groups[2].Value;
                    av.Director += data + ",";
                }

                m = Regex.Matches(res.Content, detailCompanyPattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);
                foreach (Match item in m)
                {
                    var u = item.Groups[1].Value;
                    var data = item.Groups[2].Value;
                    av.Company += data + ",";
                }

                m = Regex.Matches(res.Content, detailPublisherPattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);
                foreach (Match item in m)
                {
                    var u = item.Groups[1].Value;
                    var data = item.Groups[2].Value;
                    av.Publisher += data + ",";
                }

                m = Regex.Matches(res.Content, detailCategoryPattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);
                foreach (Match item in m)
                {
                    var data = item.Groups[2].Value;
                    av.Category += data + ",";
                }

                m = Regex.Matches(res.Content, detailActressPattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);
                foreach (Match item in m)
                {
                    var u = item.Groups[1].Value;
                    var data = item.Groups[2].Value;
                    av.Actress += data + ",";
                }

                av.URL = url;
            }
        }

        private void Fetch_Load(object sender, EventArgs e)
        {

        }
    }
}
