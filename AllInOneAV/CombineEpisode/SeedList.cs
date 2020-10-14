using Model.Common;
using Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Utils;

namespace CombineEpisode
{
    public partial class SeedList : Form
    {
        private List<SeedMagnetSearchModel> seedList = new List<SeedMagnetSearchModel>();
        private string cookieStr = "";
        private int AvId = 0;

        public SeedList()
        {
            InitializeComponent();
        }

        public SeedList(List<SeedMagnetSearchModel> seedList, string cookieStr, int avId)
        {
            InitializeComponent();

            this.seedList = seedList;
            this.cookieStr = cookieStr;
            this.AvId = avId;
        }

        private void SeedList_Load(object sender, EventArgs e)
        {
            listView1.BeginUpdate();

            foreach (var seed in seedList)
            {
                ListViewItem lvi = new ListViewItem(seed.Title);
                lvi.SubItems.Add(FileSize.GetAutoSizeString(seed.Size, 1));
                lvi.SubItems.Add(seed.CompleteCount + "");
                lvi.SubItems.Add(seed.Date.ToString("yyyy-MM-dd"));
                lvi.Tag = seed.MagUrl;

                listView1.Items.Add(lvi);
            }

            listView1.EndUpdate();
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && listView1.SelectedItems.Count > 0)
            {
                StringBuilder sb = new StringBuilder();

                foreach (ListViewItem lvi in listView1.SelectedItems)
                {
                    var mag = (string)lvi.Tag;

                    sb.AppendLine(mag);
                }

                Clipboard.SetDataObject(sb.ToString());

                var ret = OneOneFiveService.Add115MagTask(this.cookieStr, sb.ToString(), "340200422", "");

                if (ret.Item1)
                {
                    this.Close();
                }
                else
                {
                    MessageBox.Show(ret.Item2);
                }
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
