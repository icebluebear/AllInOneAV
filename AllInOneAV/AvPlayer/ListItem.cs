using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AvPlayer
{
    public partial class ListItem : UserControl
    {
        public string img = "";
        public string txt = "";
        public string avid = "";
        public string file = "";

        public ListItem(string img, string txt, string avid, string file)
        {
            InitializeComponent();
            this.img = img;
            this.txt = txt;
            this.avid = avid;
            this.file = file;

            textBox.Text = txt;
            if (!string.IsNullOrEmpty(img))
            {
                pictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
                pictureBox.Image = Image.FromFile(img);
            }
        }

        private void pictureBox_Click(object sender, EventArgs e)
        {
            
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void ListItem_Click(object sender, EventArgs e)
        {
            
        }

        private void textBox_Click(object sender, EventArgs e)
        {
            
        }

        private void pictureBox_DoubleClick(object sender, EventArgs e)
        {
            
        }

        private void pictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Clicks == 1)
            {
                MessageBox.Show(this.file);
            }
            else
            {
                MessageBox.Show("123");
            }
        }
    }
}
