using DataBaseManager.JavDataBaseHelper;
using DataBaseManager.ScanDataBaseHelper;
using Model.JavModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utils;

namespace JavBroswer
{
    public partial class Main : Form
    {
        private static string status = "{0}/{1}";
        private static string imgFolder = JavINIClass.IniReadValue("Jav", "imgFolder");
        private static int index = 0;
        private static List<ScanURL> json = new List<ScanURL>();

        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }

        private void txtLocation_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtLocation_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.RootFolder = Environment.SpecialFolder.MyComputer;
            var result = folderBrowserDialog1.ShowDialog();

            if (result == DialogResult.OK || result == DialogResult.Yes)
            {
                txtLocation.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void btnChoose_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtLocation.Text))
            {
                index = 0;
                var jsonStr = FileUtility.ReadFile(txtLocation.Text + "/update.json");
                json = JsonConvert.DeserializeObject<List<ScanURL>>(jsonStr);

                ProcessJSON();
            }
        }

        private void ShowContent(List<Model.JavModels.Comments> comment)
        {
            panel3.Controls.Clear();
            var index = 0;
            foreach (var com in comment)
            {
                var tempComment = com.Comment;

                if (com.Comment.StartsWith("[") && com.Comment.Contains("http"))
                {
                    com.Comment = com.Comment.Substring(com.Comment.IndexOf("http"));
                    tempComment = com.Comment.Substring(0, com.Comment.IndexOf("]"));
                }

                Comment c = new Comment(tempComment);
                c.Location = new Point(0, index * 70 + 1);
                c.Size = new Size(panel3.Width - 50, 70);
                panel3.Controls.Add(c);
                index++;
            }

        }

        private void ProcessJSON()
        {
            if (json.Count > 0 && json[index] != null)
            {
                labelStatus.Text = string.Format(status, index + 1, json.Count);

                var pic = imgFolder + json[index].ID + FileUtility.ReplaceInvalidChar(json[index].Title) + ".jpg";
                if (File.Exists(pic))
                {
                    pictureBox1.Image = Image.FromFile(pic);
                }

                var comments = JavDataBaseManager.GetComment(json[index].ID, FileUtility.ReplaceInvalidChar(json[index].Title));

                if (ScanDataBaseManager.HasMatch(json[index].ID))
                {
                    labelMark.BackColor = Color.Green;
                }
                else
                {
                    labelMark.BackColor = Color.Red;
                }

                ShowContent(comments);
            }
        }

        private void Main_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                if (index - 1 < 0)
                {
                    index = 0;
                }
                else
                {
                    index--;
                }

                ProcessJSON();
            }

            if(e.KeyCode == Keys.Right)
            {
                if (index + 1 >= json.Count)
                {
                    index = json.Count - 1;
                }
                else
                {
                    index++;
                }

                ProcessJSON();
            }
        }
    }
}
