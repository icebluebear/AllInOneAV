namespace DuplcateCheck
{
    partial class Form1
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.btnRe = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnRename = new System.Windows.Forms.Button();
            this.txtRename = new System.Windows.Forms.TextBox();
            this.pic3 = new System.Windows.Forms.PictureBox();
            this.pic2 = new System.Windows.Forms.PictureBox();
            this.pic1 = new System.Windows.Forms.PictureBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnExe = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtSource = new System.Windows.Forms.TextBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pic3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic1)).BeginInit();
            this.panel4.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1317, 708);
            this.panel1.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel5);
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 46);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1317, 662);
            this.panel3.TabIndex = 1;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.btnRe);
            this.panel5.Controls.Add(this.btnDelete);
            this.panel5.Controls.Add(this.btnRename);
            this.panel5.Controls.Add(this.txtRename);
            this.panel5.Controls.Add(this.pic3);
            this.panel5.Controls.Add(this.pic2);
            this.panel5.Controls.Add(this.pic1);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(0, 411);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1317, 251);
            this.panel5.TabIndex = 1;
            // 
            // btnRe
            // 
            this.btnRe.Location = new System.Drawing.Point(1217, 163);
            this.btnRe.Name = "btnRe";
            this.btnRe.Size = new System.Drawing.Size(88, 30);
            this.btnRe.TabIndex = 7;
            this.btnRe.Text = "恢复";
            this.btnRe.UseVisualStyleBackColor = true;
            this.btnRe.Click += new System.EventHandler(this.btnRe_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(1217, 107);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(88, 30);
            this.btnDelete.TabIndex = 6;
            this.btnDelete.Text = "删除";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnRename
            // 
            this.btnRename.Location = new System.Drawing.Point(1217, 43);
            this.btnRename.Name = "btnRename";
            this.btnRename.Size = new System.Drawing.Size(88, 30);
            this.btnRename.TabIndex = 5;
            this.btnRename.Text = "重命名";
            this.btnRename.UseVisualStyleBackColor = true;
            this.btnRename.Click += new System.EventHandler(this.btnRename_Click);
            // 
            // txtRename
            // 
            this.txtRename.Location = new System.Drawing.Point(985, 49);
            this.txtRename.Multiline = true;
            this.txtRename.Name = "txtRename";
            this.txtRename.Size = new System.Drawing.Size(226, 144);
            this.txtRename.TabIndex = 4;
            // 
            // pic3
            // 
            this.pic3.Location = new System.Drawing.Point(655, 6);
            this.pic3.Name = "pic3";
            this.pic3.Size = new System.Drawing.Size(320, 240);
            this.pic3.TabIndex = 3;
            this.pic3.TabStop = false;
            // 
            // pic2
            // 
            this.pic2.Location = new System.Drawing.Point(329, 6);
            this.pic2.Name = "pic2";
            this.pic2.Size = new System.Drawing.Size(320, 240);
            this.pic2.TabIndex = 2;
            this.pic2.TabStop = false;
            // 
            // pic1
            // 
            this.pic1.Location = new System.Drawing.Point(3, 6);
            this.pic1.Name = "pic1";
            this.pic1.Size = new System.Drawing.Size(320, 240);
            this.pic1.TabIndex = 1;
            this.pic1.TabStop = false;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.listView1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1317, 411);
            this.panel4.TabIndex = 0;
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(1317, 411);
            this.listView1.TabIndex = 1;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 900;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Type";
            this.columnHeader2.Width = 100;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Size";
            this.columnHeader3.Width = 120;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Length";
            this.columnHeader4.Width = 100;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnExe);
            this.panel2.Controls.Add(this.btnSearch);
            this.panel2.Controls.Add(this.txtSource);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1317, 46);
            this.panel2.TabIndex = 0;
            // 
            // btnExe
            // 
            this.btnExe.Location = new System.Drawing.Point(1245, 6);
            this.btnExe.Name = "btnExe";
            this.btnExe.Size = new System.Drawing.Size(60, 30);
            this.btnExe.TabIndex = 2;
            this.btnExe.Text = "执行";
            this.btnExe.UseVisualStyleBackColor = true;
            this.btnExe.Click += new System.EventHandler(this.btnExe_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(1166, 6);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(60, 30);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.Text = "搜索";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtSource
            // 
            this.txtSource.Location = new System.Drawing.Point(12, 12);
            this.txtSource.Name = "txtSource";
            this.txtSource.Size = new System.Drawing.Size(1148, 21);
            this.txtSource.TabIndex = 0;
            this.txtSource.Click += new System.EventHandler(this.txtSource_Click);
            this.txtSource.TextChanged += new System.EventHandler(this.txtSource_TextChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1317, 708);
            this.Controls.Add(this.panel1);
            this.KeyPreview = true;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DuplicateCheck";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pic3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic1)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtSource;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.PictureBox pic3;
        private System.Windows.Forms.PictureBox pic2;
        private System.Windows.Forms.PictureBox pic1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button btnRename;
        private System.Windows.Forms.TextBox txtRename;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnExe;
        private System.Windows.Forms.Button btnRe;
    }
}

