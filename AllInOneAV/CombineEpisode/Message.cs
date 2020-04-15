using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CombineEpisode
{
    public partial class Message : Form
    {
        private Timer timer;

        public Message()
        {
            InitializeComponent();
        }

        private void Message_Load(object sender, EventArgs e)
        {

            timer = new Timer();
            timer.Tick += delegate
            {
                this.Close();
            };
            timer.Interval = (int)TimeSpan.FromSeconds(1.5).TotalMilliseconds;
            timer.Start();
        }
    }
}
