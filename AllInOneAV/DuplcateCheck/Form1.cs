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
using Utils;

namespace DuplcateCheck
{
    public partial class Form1 : Form
    {
        private int currentIndex = -1;

        public Form1()
        {
            InitializeComponent();
            folderBrowserDialog1.RootFolder = Environment.SpecialFolder.MyComputer;
        }

        private void txtSource_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtSource_Click(object sender, EventArgs e)
        {
            var rs = folderBrowserDialog1.ShowDialog();

            if (rs == DialogResult.OK || rs == DialogResult.Yes)
            {
                txtSource.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSource.Text))
            {
                ListItem(txtSource.Text);
            }
        }

        private void ResetUi()
        {
            currentIndex = -1;
            txtRename.Text = "";
            pic1.Image = null;
            pic2.Image = null;
            pic3.Image = null;
        }

        private void ListItem(string folder)
        {
            var rawData = FileUtility.GetCheckDuplicatedData(folder);
            List<FileInfo> files = new List<FileInfo>();

            foreach (var item in rawData)
            {
                if (item.Value.Count > 1)
                {
                    files.AddRange(item.Value);
                }
            }

            files = files.OrderByDescending(x => x.Name).ToList();

            listView1.BeginUpdate();
            foreach (var file in files)
            {
                ListViewItem lvi = new ListViewItem(file.Name);
                lvi.SubItems.Add(file.Extension);
                lvi.SubItems.Add(FileSize.GetAutoSizeString(file.Length, 2));
                if (file.Extension.ToLower() != ".iso")
                {
                    lvi.SubItems.Add(FileUtility.GetDuration(file.FullName, "c:/setting/ffmpeg.exe"));
                }
                else
                {
                    lvi.SubItems.Add("-");
                }
                lvi.Tag = file;

                listView1.Items.Add(lvi);
            }

            listView1.EndUpdate();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ListView.SelectedIndexCollection indexes = listView1.SelectedIndices;
                if (indexes.Count > 0)
                {
                    ResetUi();

                    var index = indexes[0];
                    currentIndex = index;

                    FileInfo fi = (FileInfo)listView1.Items[index].Tag;

                    if (fi != null)
                    {
                        FileUtility.GetThumbnails(fi.FullName, "c:/setting/ffmpeg.exe", "c:/setting/thum/", fi.Name.Replace(fi.Extension, ""), 3, true);

                        var pic1str = "c:/setting/thum/" + fi.Name.Replace(fi.Extension, "") + "-1.jpg";
                        var pic2str = "c:/setting/thum/" + fi.Name.Replace(fi.Extension, "") + "-2.jpg";
                        var pic3str = "c:/setting/thum/" + fi.Name.Replace(fi.Extension, "") + "-3.jpg";

                        if (File.Exists(pic1str))
                        {
                            pic1.Image = Image.FromFile(pic1str);
                        }

                        if (File.Exists(pic2str))
                        {
                            pic2.Image = Image.FromFile(pic2str);
                        }

                        if (File.Exists(pic3str))
                        {
                            pic3.Image = Image.FromFile(pic3str);
                        }

                        txtRename.Text = fi.Name;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnRename_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtRename.Text) && currentIndex != -1)
            {
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    if (listView1.Items[i].BackColor != Color.Red && listView1.Items[i].SubItems[0].Text.Trim() == txtRename.Text.Trim())
                    {
                        MessageBox.Show("文件名与Index为" + (i + 1) + "项相同", "警告");
                        return;
                    }
                }

                listView1.Items[currentIndex].SubItems[0].Text = txtRename.Text;
                listView1.Items[currentIndex].BackColor = Color.Yellow;          
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (currentIndex != -1)
            {
                listView1.Items[currentIndex].BackColor = Color.Red;
            }
        }

        private void btnRe_Click(object sender, EventArgs e)
        {
            if (currentIndex != -1)
            {
                txtRename.Text = ((FileInfo)listView1.Items[currentIndex].Tag).Name;
                listView1.Items[currentIndex].Text = txtRename.Text;
                listView1.Items[currentIndex].BackColor = Color.Empty;
            }
        }

        private void btnExe_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            var folder = "c:\\setting\\checkLog\\";
            string logFile = folder + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") + "log.txt";
            long emptySpace = 0;
            int updateCount = 0;
            int deleteCount = 0;
            string rootFolder = txtSource.Text + "\\";

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            for (int i = 0; i < listView1.Items.Count; i++)
            {
                FileInfo fi = (FileInfo)listView1.Items[i].Tag;

                if (listView1.Items[i].BackColor == Color.Yellow)
                {
                    sb.AppendLine("----重命名从 " + rootFolder + fi.Name + " 到 " + rootFolder + listView1.Items[i].SubItems[0].Text);
                    updateCount++;
                }

                if (listView1.Items[i].BackColor == Color.Red)
                {
                    sb.AppendLine("****删除 " + rootFolder + fi.Name);
                    emptySpace += fi.Length;
                    deleteCount++;
                }
            }

            var res = MessageBox.Show("你将重命名 " + updateCount + " 个文件" + "\r" + "删除 " + deleteCount + " 个文件" + "\r" + "共腾出空间 " + FileSize.GetAutoSizeString(emptySpace, 2), "警告");
            StringBuilder sbError = new StringBuilder();


            if (res == DialogResult.OK || res == DialogResult.Yes)
            {        
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    try
                    {
                        FileInfo fi = (FileInfo)listView1.Items[i].Tag;

                        if (listView1.Items[i].BackColor == Color.Yellow)
                        {
                            if (File.Exists(rootFolder + listView1.Items[i].SubItems[0].Text))
                            {
                                sbError.AppendLine("--------" + listView1.Items[i].SubItems[0].Text);
                                File.Move(rootFolder + fi.Name, rootFolder + "--------" + listView1.Items[i].SubItems[0].Text);
                            }
                            else
                            {
                                File.Move(rootFolder + fi.Name, rootFolder + listView1.Items[i].SubItems[0].Text);
                            }
                        }

                        if (listView1.Items[i].BackColor == Color.Red)
                        {
                            File.Delete(rootFolder + fi.Name);
                        }
                    }
                    catch (Exception ee)
                    {
                        sbError.AppendLine(ee.ToString());
                    }
                }

                File.Create(logFile).Close();
                StreamWriter sw = new StreamWriter(logFile);
                sw.WriteLine(sb.ToString());
                sw.Close();

                listView1.Items.Clear();
                ResetUi();
            }

            MessageBox.Show(sbError.ToString());
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            ListView.SelectedIndexCollection indexes = listView1.SelectedIndices;

            if (indexes.Count > 0)
            {
                var index = indexes[0];

                FileInfo fi = (FileInfo)listView1.Items[index].Tag;

                FileUtility.PlayVideo(fi.FullName);
            }
        }
    }
}
