using Model.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utils;

namespace CombineEpisode
{
    public partial class SeedList : Form
    {
        private List<SeedMagnetSearchModel> seedList = new List<SeedMagnetSearchModel>();

        public SeedList()
        {
            InitializeComponent();
        }

        public SeedList(List<SeedMagnetSearchModel> seedList)
        {
            InitializeComponent();

            this.seedList = seedList;
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

                foreach (ListViewItem lvi in listView1.Items)
                {
                    var mag = (string)lvi.Tag;

                    sb.AppendLine(mag);
                }

                Clipboard.SetDataObject(sb.ToString());
                Message ms = new Message();
                ms.ShowDialog();
            }
        }
    }
}
