namespace AvManager
{
    partial class CompareMove
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
            this.btnOri = new System.Windows.Forms.Button();
            this.btnDes = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnOri
            // 
            this.btnOri.Location = new System.Drawing.Point(12, 12);
            this.btnOri.Name = "btnOri";
            this.btnOri.Size = new System.Drawing.Size(1939, 115);
            this.btnOri.TabIndex = 0;
            this.btnOri.Text = "button1";
            this.btnOri.UseVisualStyleBackColor = true;
            this.btnOri.Click += new System.EventHandler(this.btnOri_Click);
            // 
            // btnDes
            // 
            this.btnDes.Location = new System.Drawing.Point(12, 133);
            this.btnDes.Name = "btnDes";
            this.btnDes.Size = new System.Drawing.Size(1939, 115);
            this.btnDes.TabIndex = 1;
            this.btnDes.Text = "button2";
            this.btnDes.UseVisualStyleBackColor = true;
            this.btnDes.Click += new System.EventHandler(this.btnDes_Click);
            // 
            // CompareMove
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1963, 266);
            this.Controls.Add(this.btnDes);
            this.Controls.Add(this.btnOri);
            this.Name = "CompareMove";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CompareMove";
            this.Load += new System.EventHandler(this.CompareMove_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOri;
        private System.Windows.Forms.Button btnDes;
    }
}