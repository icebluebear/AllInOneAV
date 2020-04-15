using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JavBroswer
{
    public partial class Comment : UserControl
    {
        public string commentStr { get; set; }

        public Comment()
        {
            InitializeComponent();
        }

        public Comment(string str)
        {
            InitializeComponent();
            commentStr = str;
        }

        private void Comment_Load(object sender, EventArgs e)
        {
            this.textBox1.Text = commentStr;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                if (textBox1.Text.StartsWith("http"))
                {
                    System.Diagnostics.Process.Start("chrome.exe", textBox1.Text);
                }
                else
                {
                    Clipboard.SetDataObject(textBox1.Text);
                }
            }
        }
    }
}
