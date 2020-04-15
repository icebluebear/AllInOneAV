using DataBaseManager.FindDataBaseHelper;
using DataBaseManager.ScanDataBaseHelper;
using Model.FindModels;
using Model.ScanModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utils;

namespace FindMovie
{
    public partial class Main : Form
    {
        Thread thread;
        List<Match> cacheMovies;

        public Main()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        private void Main_Load(object sender, EventArgs e)
        {
            InitAutoComplete();
            cacheMovies = new List<Match>();
            RefreshCache();
        }

        private void findButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                listView1.Items.Clear();
                thread = new Thread(doFind);
                thread.Start();
            }
        }

        private void doFind()
        {
            InsertSearchHistory(textBox1.Text);
            doRecursive();
        }

        private void doRecursive()
        {
            listView1.BeginUpdate();

            foreach (var keyword in textBox1.Text.Split(','))
            {
                var tempKeyword = keyword.Trim().ToUpper();

                foreach (var movie in cacheMovies)
                {
                    if (movie.AvID.Trim().ToUpper().Contains(tempKeyword) || movie.AvID.Trim().ToUpper() == tempKeyword)
                    {
                        ListViewItem lvi = new ListViewItem(movie.AvID.Trim().ToUpper());

                        if (movie.Location.Length > 2 && movie.Location[1] != ':')
                        {
                            movie.Location = movie.Location.Substring(0, 1) + ":" + movie.Location.Substring(1);
                        }

                        if (File.Exists(movie.Location.Trim() + "\\" + movie.Name.Trim()))
                        {
                            var tempFi = new FileInfo(movie.Location.Trim() + "\\" + movie.Name.Trim());

                            lvi.SubItems.Add(FileSize.GetAutoSizeString(tempFi.Length, 2));
                            lvi.SubItems.Add(movie.Location.Trim() + "\\" + movie.Name.Trim());

                            listView1.Items.Add(lvi);
                        }
                    }
                }
            }

            listView1.EndUpdate();
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewHitTestInfo info = listView1.HitTest(e.X, e.Y);
            if (info.Item != null)
            {
                System.Diagnostics.Process.Start(@"" + info.Item.SubItems[2].Text);
                InsertViewHistory(info.Item.SubItems[2].Text);
            }
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ListViewHitTestInfo info = listView1.HitTest(e.X, e.Y);
                if (info.Item != null)
                {
                    var result = MessageBox.Show(string.Format("Do you want to delete {0}", info.Item.SubItems[2].Text), "Warning", MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                    {
                        File.Delete(info.Item.SubItems[2].Text);
                        RefreshCache();

                        listView1.Items.Remove(info.Item);
                    }
                }
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrEmpty(textBox1.Text))
                {
                    listView1.Items.Clear();
                    thread = new Thread(doFind);
                    thread.Start();
                }
            }
        }

        private void RefreshCache()
        {
            cacheMovies = FindDataBaseManager.GetAllMovies().OrderBy(x => x.AvID).ToList();
        }

        private void initButton_Click(object sender, EventArgs e)
        {
            RefreshCache();
        }

        private void Main_Activated(object sender, EventArgs e)
        {
            this.textBox1.Focus();
            this.textBox1.SelectAll();
        }

        private void Main_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                ListViewItem info = listView1.SelectedItems[0];

                if (info != null)
                {
                    System.Diagnostics.Process.Start(@"" + info.SubItems[2].Text);
                    InsertViewHistory(info.SubItems[2].Text);
                }
            }
        }

        private void InsertViewHistory(string file)
        {
            ScanDataBaseManager.InsertViewHistory(FileUtility.ReplaceInvalidChar(file));
        }

        private void InsertSearchHistory(string content)
        {
            ScanDataBaseManager.InsertSearchHistory(FileUtility.ReplaceInvalidChar(content));
        }

        private void InitAutoComplete()
        {
            var source = new AutoCompleteStringCollection();
            source.AddRange(ScanDataBaseManager.GetSearchHistory().Select(x => x.Content).ToArray());

            textBox1.AutoCompleteCustomSource = source;
            textBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void listView1_Validated(object sender, EventArgs e)
        {
            if (listView1.FocusedItem != null)
            {
                listView1.FocusedItem.BackColor = SystemColors.Highlight;
                listView1.FocusedItem.ForeColor = Color.White;
            }
        }

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            e.Item.ForeColor = Color.Black;
            e.Item.BackColor = SystemColors.Window;

            if (listView1.FocusedItem != null)
            {
                listView1.FocusedItem.Selected = true;
            }
        }
    }
}
