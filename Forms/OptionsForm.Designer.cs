namespace DesktopStreamDownloader
{
    partial class OptionsForm
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
            this.qualityLbl = new System.Windows.Forms.Label();
            this.qualityComboBox = new System.Windows.Forms.ComboBox();
            this.concurrentLbl = new System.Windows.Forms.Label();
            this.concurrentNum = new System.Windows.Forms.NumericUpDown();
            this.okBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.concurrentNum)).BeginInit();
            this.SuspendLayout();
            // 
            // qualityLbl
            // 
            this.qualityLbl.AutoSize = true;
            this.qualityLbl.Location = new System.Drawing.Point(9, 9);
            this.qualityLbl.Name = "qualityLbl";
            this.qualityLbl.Size = new System.Drawing.Size(79, 13);
            this.qualityLbl.TabIndex = 0;
            this.qualityLbl.Text = "&Default Quality:";
            // 
            // qualityComboBox
            // 
            this.qualityComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.qualityComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.qualityComboBox.FormattingEnabled = true;
            this.qualityComboBox.Items.AddRange(new object[] {
            "144p",
            "240p",
            "360p",
            "480p",
            "720p",
            "1080p"});
            this.qualityComboBox.Location = new System.Drawing.Point(12, 25);
            this.qualityComboBox.Name = "qualityComboBox";
            this.qualityComboBox.Size = new System.Drawing.Size(176, 21);
            this.qualityComboBox.TabIndex = 1;
            // 
            // concurrentLbl
            // 
            this.concurrentLbl.AutoSize = true;
            this.concurrentLbl.Location = new System.Drawing.Point(9, 52);
            this.concurrentLbl.Name = "concurrentLbl";
            this.concurrentLbl.Size = new System.Drawing.Size(163, 13);
            this.concurrentLbl.TabIndex = 2;
            this.concurrentLbl.Text = "Maximum Downloads at a &Time:";
            // 
            // concurrentNum
            // 
            this.concurrentNum.Location = new System.Drawing.Point(12, 68);
            this.concurrentNum.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.concurrentNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.concurrentNum.Name = "concurrentNum";
            this.concurrentNum.Size = new System.Drawing.Size(48, 20);
            this.concurrentNum.TabIndex = 3;
            this.concurrentNum.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // okBtn
            // 
            this.okBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.okBtn.Location = new System.Drawing.Point(12, 100);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(176, 23);
            this.okBtn.TabIndex = 4;
            this.okBtn.Text = "&Save";
            this.okBtn.UseVisualStyleBackColor = true;
            this.okBtn.Click += new System.EventHandler(this.okBtn_Click);
            // 
            // OptionsForm
            // 
            this.AcceptButton = this.okBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(200, 135);
            this.Controls.Add(this.okBtn);
            this.Controls.Add(this.concurrentNum);
            this.Controls.Add(this.concurrentLbl);
            this.Controls.Add(this.qualityComboBox);
            this.Controls.Add(this.qualityLbl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsForm";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Options";
            this.Load += new System.EventHandler(this.OptionsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.concurrentNum)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label qualityLbl;
        private System.Windows.Forms.ComboBox qualityComboBox;
        private System.Windows.Forms.Label concurrentLbl;
        private System.Windows.Forms.NumericUpDown concurrentNum;
        private System.Windows.Forms.Button okBtn;
    }
}
