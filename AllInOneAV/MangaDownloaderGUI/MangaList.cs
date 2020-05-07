using MangaDownloaderGUI.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MangaDownloaderGUI
{
    public partial class MangaList : Form
    {
        private List<MangaInfo> mis;
        private SourceInfo si;

        public MangaList()
        {
            InitializeComponent();
        }

        public MangaList(List<MangaInfo> mis, SourceInfo si)
        {
            this.si = si;
            this.mis = mis;
            InitializeComponent();
        }

        private void MangaList_Load(object sender, EventArgs e)
        {
            listView1.LargeImageList = imageList1;

            foreach (var m in mis)
            {
                try
                {
                    if (si.SourceName == "韩漫吧")
                    {
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(m.MangaPic);
                        request.Host = "img.manjiavip.com";
                        request.Referer = "http://www.hmba.vip/home/search";
                        imageList1.Images.Add(m.MangeUrl, Image.FromStream(request.GetResponse().GetResponseStream()));
                    }
                    else if (si.SourceName == "憨憨的漫画")
                    {
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(m.MangaPic);
                        request.Host = "i.jituoli.com";

                        if (m.MangaPic.Contains("shaque.vip"))
                        {
                            request.Host = "img001.shaque.vip";
                        }

                        request.Referer = "http://www.hanhande.net/search/?keywords=";
                        imageList1.Images.Add(m.MangeUrl, Image.FromStream(request.GetResponse().GetResponseStream()));
                    }
                    else if (si.SourceName == "污污漫画")
                    {
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(m.MangaPic);
                        request.Host = "pic.muamh.com";
                        imageList1.Images.Add(m.MangeUrl, Image.FromStream(request.GetResponse().GetResponseStream()));
                    }
                    else if (si.SourceName == "知音漫客")
                    {
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(m.MangaPic);
                        request.Host = "tu.jiayouzhibo.com";
                        imageList1.Images.Add(m.MangeUrl, Image.FromStream(request.GetResponse().GetResponseStream()));
                    }
                    else if (si.SourceName == "漫画DB")
                    {
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(m.MangaPic);
                        request.Host = "media.manhuadb.com";
                        imageList1.Images.Add(m.MangeUrl, Image.FromStream(request.GetResponse().GetResponseStream()));
                    }
                }
                catch (Exception ee)
                {
                    
                }
            }

            foreach (var m in mis)
            {
                ListViewItem lvi = new ListViewItem(m.MangaName);
                lvi.ImageIndex = imageList1.Images.IndexOfKey(m.MangeUrl);
                lvi.Tag = m;
                listView1.Items.Add(lvi);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
            {
                Main.mi = (MangaInfo)listView1.SelectedItems[0].Tag;

                this.Close();
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
