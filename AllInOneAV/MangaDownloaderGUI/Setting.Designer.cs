namespace MangaDownloaderGUI
{
    partial class Setting
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cbIsZip = new System.Windows.Forms.CheckBox();
            this.txtHistory = new System.Windows.Forms.TextBox();
            this.txtMangeDownload = new System.Windows.Forms.TextBox();
            this.txtThread = new System.Windows.Forms.TextBox();
            this.txtZipFolder = new System.Windows.Forms.TextBox();
            this.btnSettingSave = new System.Windows.Forms.Button();
            this.btnSettingCancel = new System.Windows.Forms.Button();
            this.fbdHistory = new System.Windows.Forms.FolderBrowserDialog();
            this.fbdDownload = new System.Windows.Forms.FolderBrowserDialog();
            this.fbdZip = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "下载历史信息目录";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "漫画下载目录";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "漫画下载线程数";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 178);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "压缩漫画存放目录";
            // 
            // cbIsZip
            // 
            this.cbIsZip.AutoSize = true;
            this.cbIsZip.Location = new System.Drawing.Point(14, 141);
            this.cbIsZip.Name = "cbIsZip";
            this.cbIsZip.Size = new System.Drawing.Size(120, 16);
            this.cbIsZip.TabIndex = 4;
            this.cbIsZip.Text = "是否需要压缩漫画";
            this.cbIsZip.UseVisualStyleBackColor = true;
            // 
            // txtHistory
            // 
            this.txtHistory.Location = new System.Drawing.Point(165, 27);
            this.txtHistory.Name = "txtHistory";
            this.txtHistory.Size = new System.Drawing.Size(597, 21);
            this.txtHistory.TabIndex = 5;
            this.txtHistory.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtHistory_MouseClick);
            // 
            // txtMangeDownload
            // 
            this.txtMangeDownload.Location = new System.Drawing.Point(165, 64);
            this.txtMangeDownload.Name = "txtMangeDownload";
            this.txtMangeDownload.Size = new System.Drawing.Size(597, 21);
            this.txtMangeDownload.TabIndex = 6;
            this.txtMangeDownload.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtMangeDownload_MouseClick);
            // 
            // txtThread
            // 
            this.txtThread.Location = new System.Drawing.Point(165, 101);
            this.txtThread.Name = "txtThread";
            this.txtThread.Size = new System.Drawing.Size(597, 21);
            this.txtThread.TabIndex = 7;
            this.txtThread.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtThread_KeyPress);
            // 
            // txtZipFolder
            // 
            this.txtZipFolder.Location = new System.Drawing.Point(165, 175);
            this.txtZipFolder.Name = "txtZipFolder";
            this.txtZipFolder.Size = new System.Drawing.Size(597, 21);
            this.txtZipFolder.TabIndex = 8;
            this.txtZipFolder.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtZipFolder_MouseClick);
            // 
            // btnSettingSave
            // 
            this.btnSettingSave.Location = new System.Drawing.Point(592, 205);
            this.btnSettingSave.Name = "btnSettingSave";
            this.btnSettingSave.Size = new System.Drawing.Size(75, 23);
            this.btnSettingSave.TabIndex = 9;
            this.btnSettingSave.Text = "保存";
            this.btnSettingSave.UseVisualStyleBackColor = true;
            this.btnSettingSave.Click += new System.EventHandler(this.btnSettingSave_Click);
            // 
            // btnSettingCancel
            // 
            this.btnSettingCancel.Location = new System.Drawing.Point(687, 205);
            this.btnSettingCancel.Name = "btnSettingCancel";
            this.btnSettingCancel.Size = new System.Drawing.Size(75, 23);
            this.btnSettingCancel.TabIndex = 10;
            this.btnSettingCancel.Text = "取消";
            this.btnSettingCancel.UseVisualStyleBackColor = true;
            this.btnSettingCancel.Click += new System.EventHandler(this.btnSettingCancel_Click);
            // 
            // Setting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(771, 240);
            this.Controls.Add(this.btnSettingCancel);
            this.Controls.Add(this.btnSettingSave);
            this.Controls.Add(this.txtZipFolder);
            this.Controls.Add(this.txtThread);
            this.Controls.Add(this.txtMangeDownload);
            this.Controls.Add(this.txtHistory);
            this.Controls.Add(this.cbIsZip);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Setting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "设置";
            this.Load += new System.EventHandler(this.Setting_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox cbIsZip;
        private System.Windows.Forms.TextBox txtHistory;
        private System.Windows.Forms.TextBox txtMangeDownload;
        private System.Windows.Forms.TextBox txtThread;
        private System.Windows.Forms.TextBox txtZipFolder;
        private System.Windows.Forms.Button btnSettingSave;
        private System.Windows.Forms.Button btnSettingCancel;
        private System.Windows.Forms.FolderBrowserDialog fbdHistory;
        private System.Windows.Forms.FolderBrowserDialog fbdDownload;
        private System.Windows.Forms.FolderBrowserDialog fbdZip;
    }
}