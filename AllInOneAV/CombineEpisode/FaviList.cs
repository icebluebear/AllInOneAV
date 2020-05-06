using DataBaseManager.ScanDataBaseHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CombineEpisode
{
    public partial class FaviList : Form
    {
        public FaviList()
        {
            InitializeComponent();
        }

        private void FaviList_Load(object sender, EventArgs e)
        {
            listView1.Items.Clear();

            var data = ScanDataBaseManager.GetFaviScan();

            foreach (var d in data)
            {
                ListViewItem lvi = new ListViewItem(d.Category);

                lvi.SubItems.Add(d.Name);
                lvi.SubItems.Add(d.Url);

                listView1.Items.Add(lvi);
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in listView1.SelectedItems)
            {
                Main.FaviUrls.Add(lvi.SubItems[2].Text);
            }

            this.DialogResult = DialogResult.Yes;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
