using System;
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
