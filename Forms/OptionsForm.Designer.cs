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
            this.instanceLbl = new System.Windows.Forms.Label();
            this.instanceTxtBox = new System.Windows.Forms.TextBox();
            this.okBtn = new System.Windows.Forms.Button();
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
            "240p",
            "360p",
            "480p",
            "720p",
            "1080p"});
            this.qualityComboBox.Location = new System.Drawing.Point(12, 25);
            this.qualityComboBox.Name = "qualityComboBox";
            this.qualityComboBox.Size = new System.Drawing.Size(152, 21);
            this.qualityComboBox.TabIndex = 1;
            // 
            // instanceLbl
            // 
            this.instanceLbl.AutoSize = true;
            this.instanceLbl.Location = new System.Drawing.Point(9, 49);
            this.instanceLbl.Name = "instanceLbl";
            this.instanceLbl.Size = new System.Drawing.Size(96, 13);
            this.instanceLbl.TabIndex = 2;
            this.instanceLbl.Text = "&Invidious Instance:";
            // 
            // instanceTxtBox
            // 
            this.instanceTxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.instanceTxtBox.Location = new System.Drawing.Point(12, 65);
            this.instanceTxtBox.Name = "instanceTxtBox";
            this.instanceTxtBox.Size = new System.Drawing.Size(152, 20);
            this.instanceTxtBox.TabIndex = 3;
            this.instanceTxtBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.instanceTxtBox_KeyDown);
            // 
            // okBtn
            // 
            this.okBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.okBtn.Location = new System.Drawing.Point(12, 98);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(152, 23);
            this.okBtn.TabIndex = 4;
            this.okBtn.Text = "&Save";
            this.okBtn.UseVisualStyleBackColor = true;
            this.okBtn.Click += new System.EventHandler(this.okBtn_Click);
            // 
            // OptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(176, 133);
            this.Controls.Add(this.okBtn);
            this.Controls.Add(this.instanceTxtBox);
            this.Controls.Add(this.instanceLbl);
            this.Controls.Add(this.qualityComboBox);
            this.Controls.Add(this.qualityLbl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsForm";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Options";
            this.Load += new System.EventHandler(this.OptionsForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label qualityLbl;
        private System.Windows.Forms.ComboBox qualityComboBox;
        private System.Windows.Forms.Label instanceLbl;
        private System.Windows.Forms.TextBox instanceTxtBox;
        private System.Windows.Forms.Button okBtn;
    }
}
