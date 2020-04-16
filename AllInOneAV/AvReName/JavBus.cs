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
using System.Threading.Tasks;
using System.Windows.Forms;
using Utils;

namespace AvReName
{
    public partial class JavBus : Form
    {
        private static string imgFolder = JavINIClass.IniReadValue("Jav", "imgFolder");
        private List<JavBusSearchListModel> list = new List<JavBusSearchListModel>();
        private CookieContainer cc;
        private Dictionary<string, string> mapping;
        private AV currentAv;
        private TextBox tx;

        public JavBus()
        {
            InitializeComponent();
        }

        public JavBus(List<JavBusSearchListModel> list, CookieContainer cc, Dictionary<string, string> mapping, TextBox tx)
        {
            InitializeComponent();
            this.list = list;
            this.cc = cc;
            this.mapping = mapping;
            this.tx = tx;
        }

        private void JavBus_Load(object sender, EventArgs e)
        {
            ResetUI();

            for (int i = 0; i < list.Count; i++)
            {
                imageList1.Images.Add(Image.FromStream(WebRequest.Create(list[i].Img).GetResponse().GetResponseStream()));

                ListViewItem lvi = new ListViewItem(list[i].Title);
                lvi.Tag = list[i];
                lvi.ImageIndex = i;
                listView1.LargeImageList = imageList1;
                listView1.SmallImageList = imageList1;

                listView1.Items.Add(lvi);
            }
        }

        private void ResetUI()
        {
            currentAv = null;
            pictureBox1.Image = null;
            txtTitle.Text = "";
            txtActress.Text = "";
            txtCategory.Text = "";
            txtCompany.Text = "";
            txtPublisher.Text = "";
            txtLength.Text = "";
            txtDate.Text = "";
            txtPossible.Text = "";
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var select = listView1.SelectedItems;
            if (select.Count >= 1)
            {
                ResetUI();

                var item = select[0];

                JavBusSearchListModel model = (JavBusSearchListModel)item.Tag;

                var av = Service.JavBusDownloadHelper.GetJavBusSearchDetail(model.Url, cc, mapping);

                if (av != null)
                {
                    var avs = Service.JavBusDownloadHelper.GetMatchJavDetail(av);

                    if (avs.IsMatch == false)
                    {
                        var show = Service.JavBusDownloadHelper.GetCloseLibAVModel(avs.Matches.FirstOrDefault(), mapping);

                        pictureBox1.Image = Image.FromStream(WebRequest.Create(show.PictureURL).GetResponse().GetResponseStream());
                        txtTitle.Text = show.Name;
                        txtActress.Text = show.Actress;
                        txtCategory.Text = show.Category;
                        txtCompany.Text = show.Company;
                        txtPublisher.Text = show.Publisher;
                        txtLength.Text = show.AvLength + "分钟";
                        txtDate.Text = show.ReleaseDate.ToString("yyyy-MM-dd");

                        currentAv = show;
                    }
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (currentAv != null)
            {
                currentAv.Actress = txtActress.Text.Replace("[", "").Replace("]", "");
                currentAv.Category = txtCategory.Text.Replace("[", "").Replace("]", "");
                currentAv.Company = txtCompany.Text.Replace("[", "").Replace("]", "");
                currentAv.Publisher = txtPublisher.Text.Replace("[", "").Replace("]", "");
                currentAv.ReleaseDate = DateTime.Parse(txtDate.Text);

                if (currentAv.ReleaseDate == DateTime.MinValue)
                {
                    currentAv.ReleaseDate = new DateTime(2050, 1, 1);
                }

                if (!File.Exists(imgFolder + currentAv.ID + currentAv.Name + ".jpg"))
                {
                    DownloadHelper.DownloadFile(currentAv.PictureURL, imgFolder + currentAv.ID + currentAv.Name + ".jpg");
                }

                if (!JavDataBaseManager.HasAv(currentAv.ID, currentAv.Name))
                {
                    JavDataBaseManager.InsertAV(currentAv);
                }

                tx.Text = currentAv.ID.ToUpper();
                this.DialogResult = DialogResult.Yes;
                this.Close();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void btnActress_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPossible.Text))
            {
                var list = JavDataBaseManager.GetSimilarContent("actress", txtPossible.Text);

                txtResult.Text = string.Join(",", list.Select(x=>x.Name));
            }
        }

        private void btnCategory_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPossible.Text))
            {
                var list = JavDataBaseManager.GetSimilarContent("category", txtPossible.Text);

                txtResult.Text = string.Join(",", list.Select(x => x.Name));
            }
        }

        private void btnCompany_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPossible.Text))
            {
                var list = JavDataBaseManager.GetSimilarContent("company", txtPossible.Text);

                txtResult.Text = string.Join(",", list.Select(x => x.Name));
            }
        }

        private void btnPublisher_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPossible.Text))
            {
                var list = JavDataBaseManager.GetSimilarContent("publisher", txtPossible.Text);

                txtResult.Text = string.Join(",", list.Select(x => x.Name));
            }
        }

        private void txtResult_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
