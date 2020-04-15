using DataBaseManager.SisDataBaseHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace BrowseAndDownload
{
    public partial class Main : Form
    {
        private static Dictionary<string, List<FileInfo>> list = new Dictionary<string, List<FileInfo>>();
        private static int index = 0;
        private static int picIndex = 0;
        private static Image image;
        private static List<string> picsFolder = new List<string>();
        private static List<FileInfo> fi = new List<FileInfo>();
        private static string URL = "";

        public Main()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_MouseClick(object sender, MouseEventArgs e)
        {
            folderBrowserDialog1.RootFolder = Environment.SpecialFolder.MyComputer;
            var result = folderBrowserDialog1.ShowDialog();

            if (result == DialogResult.OK || result == DialogResult.Yes)
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                index = 0;
                picIndex = 0;
                label5.Visible = false;

                fi = new List<FileInfo>();
                picsFolder = new List<string>();
                var files = Directory.GetFiles(textBox1.Text);
                foreach (var file in files)
                {
                    fi.Add(new FileInfo(file));
                }

                if (fi.Count > 0)
                {
                    SetInfo(fi);
                }
            }
        }

        private void SetInfo(List<FileInfo> fi)
        {
            URL = "";
            this.ForeColor = Color.Black;

            if (pictureBox1.Image != null)
            {
                pictureBox1.Image.Dispose();
            }

            if (image != null)
            {
                image.Dispose();
            }

            GC.Collect();

            var fullName = fi[index].FullName;
            var fileName = fi[index].Name;
            textBox3.Text = fullName;
            var picFolder = fullName.Replace(fi[index].Extension, "") + "/";

            picsFolder = Directory.GetFiles(picFolder).Where(x => x.ToLower().Contains(".jpg")).ToList();

            URL = SisDataBaseManager.GetURLFromName(fileName.Replace(fi[index].Extension, ""));

            if (picFolder.Count() > 1)
            {
                this.ForeColor = Color.Red;
            }

            try
            {
                textBox2.Text = picsFolder[picIndex];
                textBox4.Text = (picIndex + 1) + "/" + picsFolder.Count + " total " + (index + 1) + "/" + fi.Count;
                image = Image.FromFile(picsFolder[picIndex]);
                pictureBox1.Image = image;
            }
            catch (Exception e)
            {
                pictureBox1.Image = null;
            }
        }

        private void Main_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void Main_KeyDown(object sender, KeyEventArgs e)
        {
            if (fi != null && fi.Any())
            {
                if (e.KeyCode == Keys.Left)
                {
                    if (index <= 0)
                    {
                        index = 0;
                    }
                    else
                    {
                        index--;
                    }

                    picIndex = 0;
                    SetInfo(fi);
                }
                else if (e.KeyCode == Keys.Right)
                {
                    if (index >= fi.Count - 1)
                    {
                        index = fi.Count - 1;
                    }
                    else
                    {
                        index++;
                    }

                    picIndex = 0;
                    SetInfo(fi);
                }
                else if (e.KeyCode == Keys.Up)
                {
                    if (picIndex <= 0)
                    {
                        picIndex = 0;
                    }
                    else
                    {
                        picIndex--;
                    }

                    SetInfo(fi);
                }
                else if (e.KeyCode == Keys.Down)
                {
                    if(picIndex >= picsFolder.Count - 1)
                    {
                        picIndex = picsFolder.Count - 1;
                    }
                    else
                    {
                        picIndex++;
                    }

                    SetInfo(fi);
                }
                else if (e.KeyCode == Keys.Space && !string.IsNullOrEmpty(URL))
                {
                    Browser b = new Browser(URL);
                    b.ShowDialog();
                }
                else if (e.KeyCode == Keys.D || e.KeyCode == Keys.NumPad0)
                {
                    System.Diagnostics.Process.Start(textBox3.Text);
                }
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {
            label5.Visible = false;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }
    }
}