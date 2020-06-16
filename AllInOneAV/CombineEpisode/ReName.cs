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

namespace CombineEpisode
{
    public partial class ReName : Form
    {
        public string OriFile = "";

        public ReName()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                if (!File.Exists(textBox1.Text))
                {
                    File.Move(OriFile, textBox1.Text);

                    this.DialogResult = DialogResult.Yes;
                    this.Close();
                }
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ReName_Load(object sender, EventArgs e)
        {
            textBox1.Text = OriFile;
        }
    }
}
