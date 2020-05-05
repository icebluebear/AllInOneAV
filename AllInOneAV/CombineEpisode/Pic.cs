using System;
using System.Drawing;
using System.Windows.Forms;

namespace CombineEpisode
{
    public partial class Pic : Form
    {
        private string PicPath = "";

        public Pic()
        {
            InitializeComponent();
        }

        public Pic(string path)
        {
            PicPath = path;
            InitializeComponent();
        }

        private void Pic_Load(object sender, EventArgs e)
        {
            timer1.Start();
            pictureBox1.Image = Image.FromFile(PicPath);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }
    }
}
