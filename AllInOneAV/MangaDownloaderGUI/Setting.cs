using MangaDownloaderGUI.Model;
using MangaDownloaderGUI.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MangaDownloaderGUI
{
    public partial class Setting : Form
    {
        private SettingModel settingModel;

        public Setting()
        {
            InitializeComponent();
        }

        private void Setting_Load(object sender, EventArgs e)
        {
            settingModel = SettingSevice.ReadSetting();
            ShowSetting();
        }

        private void btnSettingCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtThread_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar)) && e.KeyChar != (char)8)
            {
                txtThread.Text = "请输入数字 1-20 之间的数字";
            }
        }

        private void btnSettingSave_Click(object sender, EventArgs e)
        {
            if (SaveSetting())
            {
                this.Close();
            }
            else
            {
                MessageBox.Show("保存失败");
            }
        }

        private void ShowSetting()
        {
            if (settingModel != null)
            {
                txtHistory.Text = settingModel.HistoryFolder;
                txtMangeDownload.Text = settingModel.MangaFolder;
                txtZipFolder.Text = settingModel.ZipFolder;
                txtThread.Text = settingModel.ThreadCount.ToString();
                cbIsZip.Checked = settingModel.IsZip;
            }
        }

        private bool SaveSetting()
        {
            bool ret = true;

            SettingModel sm = new SettingModel();

            try
            {
                int tc = 5;
                int.TryParse(txtThread.Text, out tc);
                var h = txtHistory.Text;
                var d = txtMangeDownload.Text;
                var z = txtZipFolder.Text;

                if (tc <= 0)
                {
                    tc = 1;
                }

                if (tc > 20)
                {
                    tc = 20;
                }

                if (!h.EndsWith("\\") && !h.EndsWith("/"))
                {
                    h += "\\";
                }

                if (!d.EndsWith("\\") && !d.EndsWith("/"))
                {
                    d += "\\";
                }

                if (!z.EndsWith("\\") && !z.EndsWith("/"))
                {
                    z += "\\";
                }

                sm.HistoryFolder = h;
                sm.MangaFolder = d;
                sm.ZipFolder = z;
                sm.ThreadCount = tc;
                sm.IsZip = cbIsZip.Checked;

                SettingSevice.WriteSetting(sm);

                MessageBox.Show("修改完目录后,如果需要继承修改前的记录,请手动复制原来目录的内容到新目录下");
            }
            catch (Exception ee)
            {
                ret = false;
                LogService.WriteLog("SaveSetting", ee.ToString());
            }

            return ret;
        }

        private void txtHistory_MouseClick(object sender, MouseEventArgs e)
        {
            fbdHistory.RootFolder = Environment.SpecialFolder.MyComputer;

            var rs = fbdHistory.ShowDialog();

            if (rs == DialogResult.OK || rs == DialogResult.Yes)
            {
                txtHistory.Text = fbdHistory.SelectedPath + "\\";
            }
        }

        private void txtMangeDownload_MouseClick(object sender, MouseEventArgs e)
        {
            fbdDownload.RootFolder = Environment.SpecialFolder.MyComputer;

            var rs = fbdDownload.ShowDialog();

            if (rs == DialogResult.OK || rs == DialogResult.Yes)
            {
                txtMangeDownload.Text = fbdDownload.SelectedPath + "\\";
            }
        }

        private void txtZipFolder_MouseClick(object sender, MouseEventArgs e)
        {
            fbdZip.RootFolder = Environment.SpecialFolder.MyComputer;

            var rs = fbdZip.ShowDialog();

            if (rs == DialogResult.OK || rs == DialogResult.Yes)
            {
                txtZipFolder.Text = fbdZip.SelectedPath + "\\";
            }
        }
    }
}
