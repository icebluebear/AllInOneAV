namespace CombineRemoveDuplicateGUI
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtSize = new System.Windows.Forms.TextBox();
            this.btnPlay = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnInit = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(2107, 1274);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(2107, 1274);
            this.panel2.TabIndex = 0;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.button1);
            this.panel4.Controls.Add(this.btnInit);
            this.panel4.Controls.Add(this.btnDelete);
            this.panel4.Controls.Add(this.btnPlay);
            this.panel4.Controls.Add(this.txtSize);
            this.panel4.Controls.Add(this.txtName);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(1289, 0);
            this.panel4.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(818, 1274);
            this.panel4.TabIndex = 2;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.treeView1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1289, 1274);
            this.panel3.TabIndex = 1;
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(1289, 1274);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(9, 12);
            this.txtName.Multiline = true;
            this.txtName.Name = "txtName";
            this.txtName.ReadOnly = true;
            this.txtName.Size = new System.Drawing.Size(797, 259);
            this.txtName.TabIndex = 0;
            // 
            // txtSize
            // 
            this.txtSize.Location = new System.Drawing.Point(9, 298);
            this.txtSize.Name = "txtSize";
            this.txtSize.ReadOnly = true;
            this.txtSize.Size = new System.Drawing.Size(797, 31);
            this.txtSize.TabIndex = 1;
            // 
            // btnPlay
            // 
            this.btnPlay.Location = new System.Drawing.Point(573, 354);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(108, 43);
            this.btnPlay.TabIndex = 2;
            this.btnPlay.Text = "播放(&P)";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(704, 354);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(102, 43);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "删除(&D)";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnInit
            // 
            this.btnInit.Location = new System.Drawing.Point(9, 354);
            this.btnInit.Name = "btnInit";
            this.btnInit.Size = new System.Drawing.Size(125, 43);
            this.btnInit.TabIndex = 4;
            this.btnInit.Text = "初始化(&I)";
            this.btnInit.UseVisualStyleBackColor = true;
            this.btnInit.Click += new System.EventHandler(this.btnInit_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(573, 444);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(102, 43);
            this.button1.TabIndex = 5;
            this.button1.Text = "自动删除";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2107, 1274);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.Name = "Form1";
            this.Text = "CombineRemoveDuplicated";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.TextBox txtSize;
        private System.Windows.Forms.Button btnInit;
        private System.Windows.Forms.Button button1;
    }
}

