namespace MangaDownloaderGUI
{
    partial class Main
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingToolStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.帮助ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelMain = new System.Windows.Forms.Panel();
            this.panelMainLog = new System.Windows.Forms.Panel();
            this.panelLog = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lvwMainList = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.rcbLog = new System.Windows.Forms.RichTextBox();
            this.panelManInfo = new System.Windows.Forms.Panel();
            this.panelInfoTotal = new System.Windows.Forms.Panel();
            this.panelInfoInfo = new System.Windows.Forms.Panel();
            this.pbSub = new System.Windows.Forms.ProgressBar();
            this.txtMainLastChapter = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.pbMain = new System.Windows.Forms.ProgressBar();
            this.txtMainStatus = new System.Windows.Forms.TextBox();
            this.txtMainChapters = new System.Windows.Forms.TextBox();
            this.txtMainUrl = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panelInfoPic = new System.Windows.Forms.Panel();
            this.picManga = new System.Windows.Forms.PictureBox();
            this.panelMainTop = new System.Windows.Forms.Panel();
            this.rbCombine = new System.Windows.Forms.RadioButton();
            this.btnMainDownload = new System.Windows.Forms.Button();
            this.btnMainSearch = new System.Windows.Forms.Button();
            this.txtMainSearch = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbDownloaded = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbSource = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.panelMain.SuspendLayout();
            this.panelMainLog.SuspendLayout();
            this.panelLog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panelManInfo.SuspendLayout();
            this.panelInfoTotal.SuspendLayout();
            this.panelInfoInfo.SuspendLayout();
            this.panelInfoPic.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picManga)).BeginInit();
            this.panelMainTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(28, 28);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.设置ToolStripMenuItem,
            this.aboutToolStrip,
            this.帮助ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 设置ToolStripMenuItem
            // 
            this.设置ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingToolStrip});
            this.设置ToolStripMenuItem.Name = "设置ToolStripMenuItem";
            this.设置ToolStripMenuItem.Size = new System.Drawing.Size(45, 20);
            this.设置ToolStripMenuItem.Text = "设置";
            // 
            // settingToolStrip
            // 
            this.settingToolStrip.Name = "settingToolStrip";
            this.settingToolStrip.Size = new System.Drawing.Size(126, 22);
            this.settingToolStrip.Text = "目录设置";
            this.settingToolStrip.Click += new System.EventHandler(this.settingToolStrip_Click);
            // 
            // aboutToolStrip
            // 
            this.aboutToolStrip.Name = "aboutToolStrip";
            this.aboutToolStrip.Size = new System.Drawing.Size(45, 20);
            this.aboutToolStrip.Text = "关于";
            this.aboutToolStrip.Click += new System.EventHandler(this.aboutToolStrip_Click);
            // 
            // 帮助ToolStripMenuItem
            // 
            this.帮助ToolStripMenuItem.Name = "帮助ToolStripMenuItem";
            this.帮助ToolStripMenuItem.Size = new System.Drawing.Size(45, 20);
            this.帮助ToolStripMenuItem.Text = "帮助";
            this.帮助ToolStripMenuItem.Click += new System.EventHandler(this.帮助ToolStripMenuItem_Click);
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.panelMainLog);
            this.panelMain.Controls.Add(this.panelManInfo);
            this.panelMain.Controls.Add(this.panelMainTop);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 24);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(800, 426);
            this.panelMain.TabIndex = 1;
            // 
            // panelMainLog
            // 
            this.panelMainLog.Controls.Add(this.panelLog);
            this.panelMainLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMainLog.Location = new System.Drawing.Point(317, 38);
            this.panelMainLog.Name = "panelMainLog";
            this.panelMainLog.Size = new System.Drawing.Size(483, 388);
            this.panelMainLog.TabIndex = 2;
            // 
            // panelLog
            // 
            this.panelLog.Controls.Add(this.splitContainer1);
            this.panelLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelLog.Location = new System.Drawing.Point(0, 0);
            this.panelLog.Name = "panelLog";
            this.panelLog.Size = new System.Drawing.Size(483, 388);
            this.panelLog.TabIndex = 2;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lvwMainList);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.rcbLog);
            this.splitContainer1.Size = new System.Drawing.Size(483, 388);
            this.splitContainer1.SplitterDistance = 160;
            this.splitContainer1.TabIndex = 0;
            // 
            // lvwMainList
            // 
            this.lvwMainList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.lvwMainList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvwMainList.FullRowSelect = true;
            this.lvwMainList.HideSelection = false;
            this.lvwMainList.Location = new System.Drawing.Point(0, 0);
            this.lvwMainList.Name = "lvwMainList";
            this.lvwMainList.Size = new System.Drawing.Size(160, 388);
            this.lvwMainList.TabIndex = 1;
            this.lvwMainList.UseCompatibleStateImageBehavior = false;
            this.lvwMainList.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "章节";
            this.columnHeader1.Width = 200;
            // 
            // rcbLog
            // 
            this.rcbLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rcbLog.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rcbLog.Location = new System.Drawing.Point(0, 0);
            this.rcbLog.Name = "rcbLog";
            this.rcbLog.Size = new System.Drawing.Size(319, 388);
            this.rcbLog.TabIndex = 2;
            this.rcbLog.Text = "";
            this.rcbLog.ContentsResized += new System.Windows.Forms.ContentsResizedEventHandler(this.rcbLog_ContentsResized);
            // 
            // panelManInfo
            // 
            this.panelManInfo.Controls.Add(this.panelInfoTotal);
            this.panelManInfo.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelManInfo.Location = new System.Drawing.Point(0, 38);
            this.panelManInfo.Name = "panelManInfo";
            this.panelManInfo.Size = new System.Drawing.Size(317, 388);
            this.panelManInfo.TabIndex = 1;
            // 
            // panelInfoTotal
            // 
            this.panelInfoTotal.Controls.Add(this.panelInfoInfo);
            this.panelInfoTotal.Controls.Add(this.panelInfoPic);
            this.panelInfoTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelInfoTotal.Location = new System.Drawing.Point(0, 0);
            this.panelInfoTotal.Name = "panelInfoTotal";
            this.panelInfoTotal.Size = new System.Drawing.Size(317, 388);
            this.panelInfoTotal.TabIndex = 0;
            // 
            // panelInfoInfo
            // 
            this.panelInfoInfo.Controls.Add(this.pbSub);
            this.panelInfoInfo.Controls.Add(this.txtMainLastChapter);
            this.panelInfoInfo.Controls.Add(this.label7);
            this.panelInfoInfo.Controls.Add(this.pbMain);
            this.panelInfoInfo.Controls.Add(this.txtMainStatus);
            this.panelInfoInfo.Controls.Add(this.txtMainChapters);
            this.panelInfoInfo.Controls.Add(this.txtMainUrl);
            this.panelInfoInfo.Controls.Add(this.label6);
            this.panelInfoInfo.Controls.Add(this.label5);
            this.panelInfoInfo.Controls.Add(this.label4);
            this.panelInfoInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelInfoInfo.Location = new System.Drawing.Point(0, 277);
            this.panelInfoInfo.Name = "panelInfoInfo";
            this.panelInfoInfo.Size = new System.Drawing.Size(317, 111);
            this.panelInfoInfo.TabIndex = 1;
            // 
            // pbSub
            // 
            this.pbSub.Location = new System.Drawing.Point(168, 85);
            this.pbSub.Name = "pbSub";
            this.pbSub.Size = new System.Drawing.Size(141, 23);
            this.pbSub.TabIndex = 10;
            // 
            // txtMainLastChapter
            // 
            this.txtMainLastChapter.Location = new System.Drawing.Point(214, 35);
            this.txtMainLastChapter.Name = "txtMainLastChapter";
            this.txtMainLastChapter.ReadOnly = true;
            this.txtMainLastChapter.Size = new System.Drawing.Size(97, 21);
            this.txtMainLastChapter.TabIndex = 9;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(155, 39);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 8;
            this.label7.Text = "上次下载";
            // 
            // pbMain
            // 
            this.pbMain.Location = new System.Drawing.Point(13, 85);
            this.pbMain.Name = "pbMain";
            this.pbMain.Size = new System.Drawing.Size(141, 23);
            this.pbMain.TabIndex = 7;
            // 
            // txtMainStatus
            // 
            this.txtMainStatus.Location = new System.Drawing.Point(57, 61);
            this.txtMainStatus.Name = "txtMainStatus";
            this.txtMainStatus.ReadOnly = true;
            this.txtMainStatus.Size = new System.Drawing.Size(254, 21);
            this.txtMainStatus.TabIndex = 6;
            // 
            // txtMainChapters
            // 
            this.txtMainChapters.Location = new System.Drawing.Point(57, 35);
            this.txtMainChapters.Name = "txtMainChapters";
            this.txtMainChapters.ReadOnly = true;
            this.txtMainChapters.Size = new System.Drawing.Size(92, 21);
            this.txtMainChapters.TabIndex = 5;
            // 
            // txtMainUrl
            // 
            this.txtMainUrl.Location = new System.Drawing.Point(57, 8);
            this.txtMainUrl.Name = "txtMainUrl";
            this.txtMainUrl.ReadOnly = true;
            this.txtMainUrl.Size = new System.Drawing.Size(254, 21);
            this.txtMainUrl.TabIndex = 4;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 65);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 12);
            this.label6.TabIndex = 3;
            this.label6.Text = "状态";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 39);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 2;
            this.label5.Text = "总共";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 1;
            this.label4.Text = "网页";
            // 
            // panelInfoPic
            // 
            this.panelInfoPic.Controls.Add(this.picManga);
            this.panelInfoPic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelInfoPic.Location = new System.Drawing.Point(0, 0);
            this.panelInfoPic.Name = "panelInfoPic";
            this.panelInfoPic.Size = new System.Drawing.Size(317, 388);
            this.panelInfoPic.TabIndex = 0;
            // 
            // picManga
            // 
            this.picManga.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picManga.Location = new System.Drawing.Point(0, 0);
            this.picManga.Name = "picManga";
            this.picManga.Size = new System.Drawing.Size(317, 388);
            this.picManga.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picManga.TabIndex = 0;
            this.picManga.TabStop = false;
            // 
            // panelMainTop
            // 
            this.panelMainTop.Controls.Add(this.rbCombine);
            this.panelMainTop.Controls.Add(this.btnMainDownload);
            this.panelMainTop.Controls.Add(this.btnMainSearch);
            this.panelMainTop.Controls.Add(this.txtMainSearch);
            this.panelMainTop.Controls.Add(this.label3);
            this.panelMainTop.Controls.Add(this.cbDownloaded);
            this.panelMainTop.Controls.Add(this.label2);
            this.panelMainTop.Controls.Add(this.cbSource);
            this.panelMainTop.Controls.Add(this.label1);
            this.panelMainTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelMainTop.Location = new System.Drawing.Point(0, 0);
            this.panelMainTop.Name = "panelMainTop";
            this.panelMainTop.Size = new System.Drawing.Size(800, 38);
            this.panelMainTop.TabIndex = 0;
            // 
            // rbCombine
            // 
            this.rbCombine.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rbCombine.AutoSize = true;
            this.rbCombine.Location = new System.Drawing.Point(579, 13);
            this.rbCombine.Name = "rbCombine";
            this.rbCombine.Size = new System.Drawing.Size(47, 16);
            this.rbCombine.TabIndex = 8;
            this.rbCombine.TabStop = true;
            this.rbCombine.Text = "合并";
            this.rbCombine.UseVisualStyleBackColor = true;
            // 
            // btnMainDownload
            // 
            this.btnMainDownload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMainDownload.Location = new System.Drawing.Point(713, 9);
            this.btnMainDownload.Name = "btnMainDownload";
            this.btnMainDownload.Size = new System.Drawing.Size(75, 23);
            this.btnMainDownload.TabIndex = 7;
            this.btnMainDownload.Text = "下载(&D)";
            this.btnMainDownload.UseVisualStyleBackColor = true;
            this.btnMainDownload.Click += new System.EventHandler(this.btnMainDownload_Click);
            // 
            // btnMainSearch
            // 
            this.btnMainSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMainSearch.Location = new System.Drawing.Point(632, 9);
            this.btnMainSearch.Name = "btnMainSearch";
            this.btnMainSearch.Size = new System.Drawing.Size(75, 23);
            this.btnMainSearch.TabIndex = 6;
            this.btnMainSearch.Text = "搜索(&S)";
            this.btnMainSearch.UseVisualStyleBackColor = true;
            this.btnMainSearch.Click += new System.EventHandler(this.btnMainSearch_Click);
            // 
            // txtMainSearch
            // 
            this.txtMainSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMainSearch.Location = new System.Drawing.Point(405, 11);
            this.txtMainSearch.Name = "txtMainSearch";
            this.txtMainSearch.Size = new System.Drawing.Size(167, 21);
            this.txtMainSearch.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(370, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "搜索";
            // 
            // cbDownloaded
            // 
            this.cbDownloaded.FormattingEnabled = true;
            this.cbDownloaded.Location = new System.Drawing.Point(231, 11);
            this.cbDownloaded.Name = "cbDownloaded";
            this.cbDownloaded.Size = new System.Drawing.Size(121, 20);
            this.cbDownloaded.TabIndex = 3;
            this.cbDownloaded.Text = "无";
            this.cbDownloaded.SelectedIndexChanged += new System.EventHandler(this.cbDownloaded_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(184, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "已下载";
            // 
            // cbSource
            // 
            this.cbSource.FormattingEnabled = true;
            this.cbSource.Location = new System.Drawing.Point(47, 11);
            this.cbSource.Name = "cbSource";
            this.cbSource.Size = new System.Drawing.Size(121, 20);
            this.cbSource.TabIndex = 1;
            this.cbSource.Text = "请选择";
            this.cbSource.SelectedIndexChanged += new System.EventHandler(this.cbSource_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "来源";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.menuStrip1);
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "漫画下载器";
            this.Load += new System.EventHandler(this.Main_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panelMain.ResumeLayout(false);
            this.panelMainLog.ResumeLayout(false);
            this.panelLog.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panelManInfo.ResumeLayout(false);
            this.panelInfoTotal.ResumeLayout(false);
            this.panelInfoInfo.ResumeLayout(false);
            this.panelInfoInfo.PerformLayout();
            this.panelInfoPic.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picManga)).EndInit();
            this.panelMainTop.ResumeLayout(false);
            this.panelMainTop.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingToolStrip;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStrip;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Panel panelMainTop;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbSource;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbDownloaded;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtMainSearch;
        private System.Windows.Forms.Button btnMainSearch;
        private System.Windows.Forms.Panel panelManInfo;
        private System.Windows.Forms.PictureBox picManga;
        private System.Windows.Forms.Panel panelInfoTotal;
        private System.Windows.Forms.Panel panelInfoPic;
        private System.Windows.Forms.Panel panelInfoInfo;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtMainStatus;
        private System.Windows.Forms.TextBox txtMainChapters;
        private System.Windows.Forms.TextBox txtMainUrl;
        private System.Windows.Forms.ProgressBar pbMain;
        private System.Windows.Forms.TextBox txtMainLastChapter;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnMainDownload;
        private System.Windows.Forms.Panel panelMainLog;
        private System.Windows.Forms.Panel panelLog;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListView lvwMainList;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.RichTextBox rcbLog;
        private System.Windows.Forms.ToolStripMenuItem 帮助ToolStripMenuItem;
        private System.Windows.Forms.ProgressBar pbSub;
        private System.Windows.Forms.RadioButton rbCombine;
    }
}

