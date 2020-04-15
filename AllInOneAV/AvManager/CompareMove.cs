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
using Utils;

namespace AvManager
{
    public partial class CompareMove : Form
    {
        public FileInfo oriFile { get; set; }
        public FileInfo desFile { get; set; }
        public string desPath { get; set; }
        public string logFile { get; set; }

        public CompareMove()
        {
            InitializeComponent();
        }

        private void CompareMove_Load(object sender, EventArgs e)
        {
            btnOri.Text = oriFile.FullName + Environment.NewLine + Utils.FileSize.GetAutoSizeString(oriFile.Length, 1);
            btnDes.Text = desFile.FullName + Environment.NewLine + Utils.FileSize.GetAutoSizeString(desFile.Length, 1);
        }

        private void btnOri_Click(object sender, EventArgs e)
        {
            desFile.Delete();
            oriFile.MoveTo(desPath + oriFile.Name);

            LogHelper.WriteLog(logFile, "删除 -> " + desFile.FullName + " 移动 " + oriFile.FullName + " -> " + desPath + oriFile.Name);
            this.Close();
        }

        private void btnDes_Click(object sender, EventArgs e)
        {
            oriFile.Delete();

            LogHelper.WriteLog(logFile, "删除 -> " + oriFile.FullName);
            this.Close();
        }
    }
}
