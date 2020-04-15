using DataBaseManager.ScanDataBaseHelper;
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

namespace PlayRecent
{
    public partial class Main : Form
    {
        private static List<string> formats = JavINIClass.IniReadValue("Scan", "Format").Split(',').ToList();
        private static List<string> excludes = JavINIClass.IniReadValue("Scan", "Exclude").Split(',').ToList();
        private static List<FileInfo> fi = new List<FileInfo>();

        public Main()
        {
            InitializeComponent();
        }

        private void btnBroswe_Click(object sender, EventArgs e)
        {
            ListItems();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void ListItems()
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                fi = new List<FileInfo>();
                this.listView1.Items.Clear();

                FileUtility.GetFilesRecursive(textBox1.Text, formats, excludes, fi, 100);

                WhenClickRb();
                ShowContent();
            }
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.RootFolder = Environment.SpecialFolder.MyComputer;

            var result = folderBrowserDialog1.ShowDialog();

            if (result == DialogResult.OK || result == DialogResult.Yes)
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var current = listView1.GetItemAt(e.X, e.Y);

            Play(current);
        }

        private void Main_Load(object sender, EventArgs e)
        {
           
        }

        private void rbDateD_CheckedChanged(object sender, EventArgs e)
        {
            WhenClickRb();
            ShowContent();
        }

        private void rbDateA_CheckedChanged(object sender, EventArgs e)
        {
            WhenClickRb();
            ShowContent();
        }

        private void rbSizeD_CheckedChanged(object sender, EventArgs e)
        {
            WhenClickRb();
            ShowContent();
        }

        private void rbSizeA_CheckedChanged(object sender, EventArgs e)
        {
            WhenClickRb();
            ShowContent();
        }

        private void Main_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                if (listView1.SelectedItems != null && listView1.SelectedItems.Count > 0)
                {
                    if (listView1.SelectedItems.Count == 1)
                    {
                        Play(listView1.SelectedItems[0]);
                    }
                    else
                    {
                        List<string> list = new List<string>();

                        foreach (ListViewItem item in listView1.SelectedItems)
                        {
                            list.Add(item.Tag.ToString());

                            item.BackColor = Color.Green;
                            ScanDataBaseManager.InsertViewHistory(FileUtility.ReplaceInvalidChar(item.Tag + ""));
                        }

                        PlayPlist(Utils.PlayerHelper.GeneratePotPlayerPlayList(list));
                    }
                }
            }

            if (e.KeyCode == Keys.Enter)
            {
                ListItems();
            }

            if (e.KeyCode == Keys.D)
            {
                if (listView1.SelectedItems != null && listView1.SelectedItems.Count > 0)
                {
                    Delete(listView1.SelectedItems[0]);
                }
            }

            if (e.KeyCode == Keys.NumPad0)
            {
                if (listView1.SelectedItems != null && listView1.SelectedItems.Count > 0)
                {
                    Delete(listView1.SelectedItems[0]);
                }
            }
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var current = listView1.GetItemAt(e.X, e.Y);

                Delete(current);
            }
        }

        private void ShowContent()
        {
            listView1.Items.Clear();

            listView1.BeginUpdate();
            foreach (var file in fi)
            {
                ListViewItem lvi = new ListViewItem(file.DirectoryName);
                lvi.SubItems.Add(file.Name);
                lvi.SubItems.Add(FileSize.GetAutoSizeString(file.Length, 1));
                lvi.Tag = file.FullName;

                if (ScanDataBaseManager.ViewedFile(FileUtility.ReplaceInvalidChar(file.FullName)))
                {
                    lvi.BackColor = Color.Green;
                }

                listView1.Items.Add(lvi);
            }
            listView1.EndUpdate();
        }

        private void WhenClickRb()
        {
            if (fi != null && fi.Count > 0)
            {
                if (rbDateA.Checked)
                {
                    fi = fi.OrderBy(x => x.LastWriteTime).ToList();
                }

                if (rbDateD.Checked)
                {
                    fi = fi.OrderByDescending(x => x.LastWriteTime).ToList();
                }

                if (rbSizeA.Checked)
                {
                    fi = fi.OrderBy(x => x.Length).ToList();
                }

                if (rbSizeD.Checked)
                {
                    fi = fi.OrderByDescending(x => x.Length).ToList();
                }
            }
        }

        private void Play(ListViewItem current)
        {
            if (current != null)
            {
                System.Diagnostics.Process.Start(@"" + current.Tag);
                current.BackColor = Color.Green;
                ScanDataBaseManager.InsertViewHistory(FileUtility.ReplaceInvalidChar(current.Tag + ""));
            }
        }

        private void PlayPlist(string files)
        {
            System.Diagnostics.Process.Start(@"" + files);
        }

        private void Delete(ListViewItem current)
        {
            var res = MessageBox.Show(string.Format("Do you want to delete {0}", current.Tag), "Warrning", MessageBoxButtons.YesNo);

            if (res == DialogResult.Yes)
            {
                File.Delete(current.Tag + "");

                listView1.Items.Remove(current);
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
