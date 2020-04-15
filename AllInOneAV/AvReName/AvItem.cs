using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AvReName
{
    public partial class AvItem : UserControl
    {
        public string AvId { get; set; }
        public string AvName { get; set; }
        public string ImageStr { get; set; }
        public Panel Panel {
            get {
                return this.panel2;
            }
        }
        public delegate void AvItemClickHandle(object sender, EventArgs e);

        public AvItem()
        {
            InitializeComponent();
        }

        public AvItem(string name, string img, string id)
        {
            InitializeComponent();
            avItemLabel.Text = name;
            picBox.Image = Image.FromFile(img);

            AvName = name;
            ImageStr = img;
            AvId = id;
        }

        public event AvItemClickHandle AvItemClicked;

        private void avitem_Click(object sender, EventArgs e)
        {
            
        }

        private void picBox_Click(object sender, EventArgs e)
        {
            AvItemClicked?.Invoke(sender, new EventArgs());//把按钮自身作为参数传递
        }

        private void AvItem_Click_1(object sender, EventArgs e)
        {
            
        }
    }
}
