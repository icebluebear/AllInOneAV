using System;
using System.Windows.Forms;

namespace BrowseAndDownload
{
    public partial class Browser : Form
    {
        public Uri uri;

        public Browser()
        {
            InitializeComponent();
        }

        public Browser(string uri)
        {
            this.uri = new Uri(uri);
            InitializeComponent();
        }

        private void Browser_Load(object sender, EventArgs e)
        {
            webBrowser1.Url = this.uri;
        }

        private void Browser_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.C)
            {
                //Form.ActiveForm.Close();
                this.Close();
                this.Dispose();
            }
        }
    }
}
