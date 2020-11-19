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
    public partial class Thums : Form
    {
        public List<string> pics = new List<string>();

        public Thums()
        {
            InitializeComponent();
        }

        public Thums(List<string> pics)
        {
            InitializeComponent();
            this.pics = pics;
        }

        private void Thums_Load(object sender, EventArgs e)
        {
            foreach (var l in pics)
            {
                if (File.Exists(l))
                {
                    imThums.Images.Add(l, Image.FromFile(l));
                }
            }

            foreach (var l in pics)
            {
                ListViewItem lvi = new ListViewItem(l)
                {
                    ImageIndex = imThums.Images.IndexOfKey(l),
                };

                listView1.Items.Add(lvi);
            }
        }
    }
}
