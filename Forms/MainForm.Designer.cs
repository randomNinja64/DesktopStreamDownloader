namespace DesktopStreamDownloader
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.searchTxtBox = new System.Windows.Forms.TextBox();
            this.searchBtn = new System.Windows.Forms.Button();
            this.controlTabs = new System.Windows.Forms.TabControl();
            this.searchTab = new System.Windows.Forms.TabPage();
            this.downloadButton = new System.Windows.Forms.Button();
            this.resultsGrid = new System.Windows.Forms.DataGridView();
            this.resultName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.identifier = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Views = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.resultDescription = new System.Windows.Forms.TextBox();
            this.resultPreview = new System.Windows.Forms.PictureBox();
            this.downloadTab = new System.Windows.Forms.TabPage();
            this.openDownloadsBtn = new System.Windows.Forms.Button();
            this.optionsBtn = new System.Windows.Forms.Button();
            this.cancelDlButton = new System.Windows.Forms.Button();
            this.downloadsDataGridView = new System.Windows.Forms.DataGridView();
            this.fileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.downloadStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DownloadProgress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dlDirLabel = new System.Windows.Forms.Label();
            this.dlDirTxtBox = new System.Windows.Forms.TextBox();
            this.setDirBtn = new System.Windows.Forms.Button();
            this.keywordLbl = new System.Windows.Forms.Label();
            this.pagesLbl = new System.Windows.Forms.Label();
            this.resultsNum = new System.Windows.Forms.NumericUpDown();
            this.defaultStatusStrip = new System.Windows.Forms.StatusStrip();
            this.queueStatusLbl = new System.Windows.Forms.ToolStripStatusLabel();
            this.controlTabs.SuspendLayout();
            this.searchTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.resultsGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.resultPreview)).BeginInit();
            this.downloadTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.downloadsDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.resultsNum)).BeginInit();
            this.defaultStatusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // searchTxtBox
            // 
            this.searchTxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.searchTxtBox.Location = new System.Drawing.Point(12, 26);
            this.searchTxtBox.Name = "searchTxtBox";
            this.searchTxtBox.Size = new System.Drawing.Size(544, 20);
            this.searchTxtBox.TabIndex = 1;
            this.searchTxtBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.searchTxtBox_KeyDown);
            // 
            // searchBtn
            // 
            this.searchBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.searchBtn.Location = new System.Drawing.Point(562, 26);
            this.searchBtn.Name = "searchBtn";
            this.searchBtn.Size = new System.Drawing.Size(76, 21);
            this.searchBtn.TabIndex = 2;
            this.searchBtn.Text = "&Search";
            this.searchBtn.UseVisualStyleBackColor = true;
            this.searchBtn.Click += new System.EventHandler(this.searchBtn_Click);
            // 
            // controlTabs
            // 
            this.controlTabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.controlTabs.Controls.Add(this.searchTab);
            this.controlTabs.Controls.Add(this.downloadTab);
            this.controlTabs.Location = new System.Drawing.Point(12, 52);
            this.controlTabs.Name = "controlTabs";
            this.controlTabs.SelectedIndex = 0;
            this.controlTabs.Size = new System.Drawing.Size(721, 408);
            this.controlTabs.TabIndex = 5;
            // 
            // searchTab
            // 
            this.searchTab.Controls.Add(this.downloadButton);
            this.searchTab.Controls.Add(this.resultsGrid);
            this.searchTab.Controls.Add(this.resultDescription);
            this.searchTab.Controls.Add(this.resultPreview);
            this.searchTab.Location = new System.Drawing.Point(4, 22);
            this.searchTab.Name = "searchTab";
            this.searchTab.Padding = new System.Windows.Forms.Padding(3);
            this.searchTab.Size = new System.Drawing.Size(713, 382);
            this.searchTab.TabIndex = 0;
            this.searchTab.Text = "Search Results";
            this.searchTab.UseVisualStyleBackColor = true;
            // 
            // downloadButton
            // 
            this.downloadButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.downloadButton.Location = new System.Drawing.Point(511, 349);
            this.downloadButton.Name = "downloadButton";
            this.downloadButton.Size = new System.Drawing.Size(196, 27);
            this.downloadButton.TabIndex = 0;
            this.downloadButton.Text = "&Add To Queue";
            this.downloadButton.UseVisualStyleBackColor = true;
            this.downloadButton.Click += new System.EventHandler(this.downloadButton_Click);
            // 
            // resultsGrid
            // 
            this.resultsGrid.AllowUserToAddRows = false;
            this.resultsGrid.AllowUserToDeleteRows = false;
            this.resultsGrid.AllowUserToOrderColumns = true;
            this.resultsGrid.AllowUserToResizeColumns = false;
            this.resultsGrid.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.resultsGrid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.resultsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.resultsGrid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.resultsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.resultsGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.resultName,
            this.identifier,
            this.description,
            this.Views});
            this.resultsGrid.Location = new System.Drawing.Point(6, 6);
            this.resultsGrid.MultiSelect = false;
            this.resultsGrid.Name = "resultsGrid";
            this.resultsGrid.ReadOnly = true;
            this.resultsGrid.RowHeadersVisible = false;
            this.resultsGrid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.resultsGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.resultsGrid.Size = new System.Drawing.Size(499, 370);
            this.resultsGrid.TabIndex = 2;
            this.resultsGrid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.resultsGrid_CellDoubleClick);
            this.resultsGrid.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.resultsGrid_RowEnter);
            this.resultsGrid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.resultsGrid_KeyDown);
            // 
            // resultName
            // 
            this.resultName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.resultName.HeaderText = "Name";
            this.resultName.MinimumWidth = 120;
            this.resultName.Name = "resultName";
            this.resultName.ReadOnly = true;
            // 
            // identifier
            // 
            this.identifier.HeaderText = "Identifier";
            this.identifier.Name = "identifier";
            this.identifier.ReadOnly = true;
            this.identifier.Visible = false;
            // 
            // description
            // 
            this.description.HeaderText = "Description";
            this.description.Name = "description";
            this.description.ReadOnly = true;
            this.description.Visible = false;
            // 
            // Views
            // 
            this.Views.HeaderText = "Views";
            this.Views.Name = "Views";
            this.Views.ReadOnly = true;
            // 
            // resultDescription
            // 
            this.resultDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.resultDescription.BackColor = System.Drawing.SystemColors.Window;
            this.resultDescription.Location = new System.Drawing.Point(511, 122);
            this.resultDescription.Multiline = true;
            this.resultDescription.Name = "resultDescription";
            this.resultDescription.ReadOnly = true;
            this.resultDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.resultDescription.Size = new System.Drawing.Size(196, 221);
            this.resultDescription.TabIndex = 1;
            // 
            // resultPreview
            // 
            this.resultPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.resultPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.resultPreview.Location = new System.Drawing.Point(512, 6);
            this.resultPreview.Name = "resultPreview";
            this.resultPreview.Size = new System.Drawing.Size(194, 110);
            this.resultPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.resultPreview.TabIndex = 1;
            this.resultPreview.TabStop = false;
            // 
            // downloadTab
            // 
            this.downloadTab.Controls.Add(this.openDownloadsBtn);
            this.downloadTab.Controls.Add(this.optionsBtn);
            this.downloadTab.Controls.Add(this.cancelDlButton);
            this.downloadTab.Controls.Add(this.downloadsDataGridView);
            this.downloadTab.Controls.Add(this.dlDirLabel);
            this.downloadTab.Controls.Add(this.dlDirTxtBox);
            this.downloadTab.Controls.Add(this.setDirBtn);
            this.downloadTab.Location = new System.Drawing.Point(4, 22);
            this.downloadTab.Name = "downloadTab";
            this.downloadTab.Padding = new System.Windows.Forms.Padding(3);
            this.downloadTab.Size = new System.Drawing.Size(713, 382);
            this.downloadTab.TabIndex = 1;
            this.downloadTab.Text = "Downloads";
            this.downloadTab.UseVisualStyleBackColor = true;
            // 
            // openDownloadsBtn
            // 
            this.openDownloadsBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.openDownloadsBtn.Location = new System.Drawing.Point(354, 354);
            this.openDownloadsBtn.Name = "openDownloadsBtn";
            this.openDownloadsBtn.Size = new System.Drawing.Size(104, 23);
            this.openDownloadsBtn.TabIndex = 2;
            this.openDownloadsBtn.Text = "Open &Downloads";
            this.openDownloadsBtn.UseVisualStyleBackColor = true;
            this.openDownloadsBtn.Click += new System.EventHandler(this.openDownloadsBtn_Click);
            // 
            // optionsBtn
            // 
            this.optionsBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.optionsBtn.Location = new System.Drawing.Point(546, 354);
            this.optionsBtn.Name = "optionsBtn";
            this.optionsBtn.Size = new System.Drawing.Size(76, 23);
            this.optionsBtn.TabIndex = 4;
            this.optionsBtn.Text = "&Options";
            this.optionsBtn.UseVisualStyleBackColor = true;
            this.optionsBtn.Click += new System.EventHandler(this.optionsBtn_Click);
            // 
            // cancelDlButton
            // 
            this.cancelDlButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelDlButton.Enabled = false;
            this.cancelDlButton.Location = new System.Drawing.Point(628, 354);
            this.cancelDlButton.Name = "cancelDlButton";
            this.cancelDlButton.Size = new System.Drawing.Size(76, 23);
            this.cancelDlButton.TabIndex = 5;
            this.cancelDlButton.Text = "&Cancel";
            this.cancelDlButton.UseVisualStyleBackColor = true;
            this.cancelDlButton.Click += new System.EventHandler(this.cancelDlButton_Click);
            // 
            // downloadsDataGridView
            // 
            this.downloadsDataGridView.AllowUserToAddRows = false;
            this.downloadsDataGridView.AllowUserToDeleteRows = false;
            this.downloadsDataGridView.AllowUserToOrderColumns = true;
            this.downloadsDataGridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.downloadsDataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            this.downloadsDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.downloadsDataGridView.BackgroundColor = System.Drawing.SystemColors.Control;
            this.downloadsDataGridView.AutoGenerateColumns = false;
            this.downloadsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.downloadsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.fileName,
            this.downloadStatus,
            this.DownloadProgress});
            this.downloadsDataGridView.Location = new System.Drawing.Point(6, 6);
            this.downloadsDataGridView.MultiSelect = false;
            this.downloadsDataGridView.Name = "downloadsDataGridView";
            this.downloadsDataGridView.ReadOnly = true;
            this.downloadsDataGridView.RowHeadersVisible = false;
            this.downloadsDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.downloadsDataGridView.Size = new System.Drawing.Size(701, 332);
            this.downloadsDataGridView.TabIndex = 6;
            this.downloadsDataGridView.SelectionChanged += new System.EventHandler(this.downloadsDataGridView_SelectionChanged);
            // 
            // fileName
            // 
            this.fileName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.fileName.DataPropertyName = "fileName";
            this.fileName.HeaderText = "File Name";
            this.fileName.Name = "fileName";
            this.fileName.ReadOnly = true;
            // 
            // downloadStatus
            // 
            this.downloadStatus.DataPropertyName = "downloadStatus";
            this.downloadStatus.HeaderText = "Status";
            this.downloadStatus.Name = "downloadStatus";
            this.downloadStatus.ReadOnly = true;
            this.downloadStatus.Width = 140;
            // 
            // DownloadProgress
            // 
            this.DownloadProgress.DataPropertyName = "downloadProgress";
            this.DownloadProgress.HeaderText = "Progress";
            this.DownloadProgress.Name = "DownloadProgress";
            this.DownloadProgress.ReadOnly = true;
            this.DownloadProgress.Width = 140;
            // 
            // dlDirLabel
            // 
            this.dlDirLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.dlDirLabel.AutoSize = true;
            this.dlDirLabel.Location = new System.Drawing.Point(3, 341);
            this.dlDirLabel.Name = "dlDirLabel";
            this.dlDirLabel.Size = new System.Drawing.Size(108, 13);
            this.dlDirLabel.TabIndex = 0;
            this.dlDirLabel.Text = "&Downloads Directory:";
            // 
            // dlDirTxtBox
            // 
            this.dlDirTxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dlDirTxtBox.Location = new System.Drawing.Point(6, 356);
            this.dlDirTxtBox.Name = "dlDirTxtBox";
            this.dlDirTxtBox.Size = new System.Drawing.Size(342, 20);
            this.dlDirTxtBox.TabIndex = 1;
            // 
            // setDirBtn
            // 
            this.setDirBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.setDirBtn.Location = new System.Drawing.Point(464, 354);
            this.setDirBtn.Name = "setDirBtn";
            this.setDirBtn.Size = new System.Drawing.Size(76, 23);
            this.setDirBtn.TabIndex = 3;
            this.setDirBtn.Text = "&Browse...";
            this.setDirBtn.UseVisualStyleBackColor = true;
            this.setDirBtn.Click += new System.EventHandler(this.setDirBtn_Click);
            // 
            // keywordLbl
            // 
            this.keywordLbl.AutoSize = true;
            this.keywordLbl.Location = new System.Drawing.Point(9, 10);
            this.keywordLbl.Name = "keywordLbl";
            this.keywordLbl.Size = new System.Drawing.Size(75, 13);
            this.keywordLbl.TabIndex = 0;
            this.keywordLbl.Text = "Search &Query:";
            // 
            // pagesLbl
            // 
            this.pagesLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pagesLbl.AutoSize = true;
            this.pagesLbl.Location = new System.Drawing.Point(641, 10);
            this.pagesLbl.Name = "pagesLbl";
            this.pagesLbl.Size = new System.Drawing.Size(92, 13);
            this.pagesLbl.TabIndex = 3;
            this.pagesLbl.Text = "&Maximum Results:";
            // 
            // resultsNum
            // 
            this.resultsNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.resultsNum.Location = new System.Drawing.Point(644, 26);
            this.resultsNum.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.resultsNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.resultsNum.Name = "resultsNum";
            this.resultsNum.Size = new System.Drawing.Size(89, 20);
            this.resultsNum.TabIndex = 4;
            this.resultsNum.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // defaultStatusStrip
            // 
            this.defaultStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.queueStatusLbl});
            this.defaultStatusStrip.Location = new System.Drawing.Point(0, 463);
            this.defaultStatusStrip.Name = "defaultStatusStrip";
            this.defaultStatusStrip.Size = new System.Drawing.Size(745, 22);
            this.defaultStatusStrip.TabIndex = 6;
            this.defaultStatusStrip.Text = "statusStrip1";
            // 
            // queueStatusLbl
            // 
            this.queueStatusLbl.Name = "queueStatusLbl";
            this.queueStatusLbl.Size = new System.Drawing.Size(0, 17);
            this.queueStatusLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(745, 485);
            this.Controls.Add(this.defaultStatusStrip);
            this.Controls.Add(this.resultsNum);
            this.Controls.Add(this.pagesLbl);
            this.Controls.Add(this.keywordLbl);
            this.Controls.Add(this.controlTabs);
            this.Controls.Add(this.searchBtn);
            this.Controls.Add(this.searchTxtBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(563, 378);
            this.Name = "MainForm";
            this.Text = "DesktopStreamDownloader 1.3.5";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.controlTabs.ResumeLayout(false);
            this.searchTab.ResumeLayout(false);
            this.searchTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.resultsGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.resultPreview)).EndInit();
            this.downloadTab.ResumeLayout(false);
            this.downloadTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.downloadsDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.resultsNum)).EndInit();
            this.defaultStatusStrip.ResumeLayout(false);
            this.defaultStatusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox searchTxtBox;
        private System.Windows.Forms.Button searchBtn;
        private System.Windows.Forms.TabControl controlTabs;
        private System.Windows.Forms.TabPage searchTab;
        private System.Windows.Forms.TabPage downloadTab;
        private System.Windows.Forms.TextBox resultDescription;
        private System.Windows.Forms.PictureBox resultPreview;
        private System.Windows.Forms.DataGridView resultsGrid;
        private System.Windows.Forms.Button downloadButton;
        private System.Windows.Forms.Button setDirBtn;
        private System.Windows.Forms.Label dlDirLabel;
        private System.Windows.Forms.TextBox dlDirTxtBox;
        public System.Windows.Forms.DataGridView downloadsDataGridView;
        private System.Windows.Forms.Button cancelDlButton;
        private System.Windows.Forms.Button optionsBtn;
        private System.Windows.Forms.DataGridViewTextBoxColumn resultName;
        private System.Windows.Forms.DataGridViewTextBoxColumn identifier;
        private System.Windows.Forms.DataGridViewTextBoxColumn description;
        private System.Windows.Forms.DataGridViewTextBoxColumn Views;
        private System.Windows.Forms.DataGridViewTextBoxColumn fileName;
        private System.Windows.Forms.DataGridViewTextBoxColumn downloadStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn DownloadProgress;
        private System.Windows.Forms.Label keywordLbl;
        private System.Windows.Forms.Label pagesLbl;
        private System.Windows.Forms.NumericUpDown resultsNum;
        private System.Windows.Forms.StatusStrip defaultStatusStrip;
        internal System.Windows.Forms.ToolStripStatusLabel queueStatusLbl;
        private System.Windows.Forms.Button openDownloadsBtn;
    }
}

