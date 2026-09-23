using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DesktopStreamDownloader
{

    public partial class MainForm : Form
    {
        // Create a new Download Handler
        DownloadHandler downloadHandler = new DownloadHandler();
        
        static MainForm _frmObj;
        public static MainForm frmObj
        {
            get { return _frmObj; }
            set { _frmObj = value; }
        }


        public MainForm()
        {
            InitializeComponent();
        }

        public void UpdateAfterDownload()
        {
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Set the form object
            frmObj = this;

            // If a download path hasn't been set, prompt the user for one
            if (Properties.Settings.Default.DownloadPath == "")
            {
                if (setDownloadPath() == 1) 
                {
                    Application.Exit();
                }
                
            }

            // Save default settings
            Properties.Settings.Default.Save();

            // Place download path in appropriate control
            dlDirTxtBox.Text = Properties.Settings.Default.DownloadPath;

            // Set Sort Column of resultsGrid to Name
            resultsGrid.Sort(resultsGrid.Columns[0], System.ComponentModel.ListSortDirection.Ascending);

            // Setup Downloads Grid View
            // Create Binding Source For Download Manager
            BindingSource downloadBindingSource = new BindingSource
            {
                // Set the Binding Source to the Download Handler
                DataSource = downloadHandler.Downloads
            };
            // Set the Data Source of the Download Manager to the Binding Source
            downloadsDataGridView.DataSource = downloadBindingSource;

        }

        private void searchBtn_Click(object sender, EventArgs e)
        {
            // Switch To Search Tab
            controlTabs.SelectedTab = searchTab;

            // Clear Search Results
            resultsGrid.Rows.Clear();

            // Notify user that search is happening via info text box
            resultDescription.Text = "Grabbing Results. Please wait...";

            // Perform Search With InvidiousHandler
            List<InvidiousHandler.VideoItem> results = InvidiousHandler.Search(searchTxtBox.Text, Convert.ToInt32(resultsNum.Value));

            // Set result description back to blank
            resultDescription.Text = "";

            // If results is null, break
            if (results == null)
            {
                return;
            }

            // Add results to resultsGrid if results is not empty
            if (results.Count > 0)
            {
                foreach (InvidiousHandler.VideoItem result in results)
                {
                    resultsGrid.Rows.Add(result.title, result.identifier, result.description, result.views);
                }
            }
        }

        private void searchTxtBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void searchTxtBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Suppress alert noise
                e.SuppressKeyPress = true;
                searchBtn_Click(this, new EventArgs());
            }
        }

        private void resultsGrid_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            // Set the description textbox's text to the description cell of the currently selected row
            resultDescription.Text = resultsGrid.Rows[e.RowIndex].Cells[2].Value.ToString();

            if (e.RowIndex != -1)
            {
                // Get identifier of selected item
                string identifier = resultsGrid.Rows[e.RowIndex].Cells[1].Value.ToString();

                // Download thumbnail
                resultPreview.ImageLocation = InvidiousHandler.GetThumbnailUrl(identifier);

                // If description text box is blank, set it to "No description found."
                if (resultDescription.Text == "")
                {
                    resultDescription.Text = "No description found.";
                }

            }
        }

        private void downloadButton_Click(object sender, EventArgs e)
        {
            //If an item is selected, Open DownloadForm and pass in the selected item's identifier
            if (resultsGrid.SelectedRows.Count > 0)
            {
                // Create filename (video title + .mp4)
                string filename = resultsGrid.SelectedRows[0].Cells[0].Value.ToString() + ".mp4";

                // Enumerate items in Downloads tab before dialog
                //int numDownloads = downloadHandler.Downloads.Count;

                //MessageBox.Show(InvidiousHandler.GetVideoUrl(resultsGrid.SelectedRows[0].Cells[1].Value.ToString()));
                Uri URL = new Uri(InvidiousHandler.GetVideoUrl(resultsGrid.SelectedRows[0].Cells[1].Value.ToString()));

                // If URL is null, error out and break
                if (URL == null)
                {
                    MessageBox.Show("Error 31: No MP4 files found for video.");
                    return;
                }

                // Set status label to indicate item added to queue
                queueStatusLbl.Text = "Added " + resultsGrid.SelectedRows[0].Cells[0].Value.ToString() + " to queue.";

                downloadHandler.addDownload(URL, filename, progressTimer);

                // Change selected tab to downloads tab if items were downloaded
                /*if (downloadHandler.Downloads.Count > numDownloads)
                {
                    controlTabs.SelectedTab = downloadTab;
                }*/

            }
        }

        private void resultsGrid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Run resultsGridCellDoubleClick
                resultsGrid_CellDoubleClick(this, new DataGridViewCellEventArgs(resultsGrid.CurrentCell.ColumnIndex, resultsGrid.CurrentCell.RowIndex));
            }
        }

        private void dlDirTxtBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void clrInactiveBtn_Click(object sender, EventArgs e)
        {
        }

        private void setDirBtn_Click(object sender, EventArgs e)
        {
            setDownloadPath();

            //Update Text Box
            dlDirTxtBox.Text = Properties.Settings.Default.DownloadPath;

            // Save Properties
            Properties.Settings.Default.Save();
        }

        private int setDownloadPath()
        {
            // Create a new folder browser dialog
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog
            {
                // Set the description
                Description = "Please select path for downloaded files."
            };

            // Show the dialog
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                // Set the download path
                Properties.Settings.Default.DownloadPath = folderBrowserDialog.SelectedPath;
                return 0;
            }
            else
            {
                return 1;
            }
        }
        private void searchTab_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            downloadsDataGridView.Refresh();
        }

        private void cancelDlButton_Click(object sender, EventArgs e)
        {
            // If no row is selected, disable cancel button and return
            if (downloadsDataGridView.SelectedRows.Count == 0)
            {
                cancelDlButton.Enabled = false;
                return;
            }
            
            // If a download is running, abort it and delete the file
            if (downloadHandler.Downloads.Count > 0)
            {
                if (downloadsDataGridView.SelectedRows[0].Index > 0 && downloadsDataGridView.SelectedRows[0].Index < downloadsDataGridView.Rows.Count)
                {
                    downloadHandler.removeDownloadAtIndex(downloadsDataGridView.SelectedRows[0].Index);
                } else
                {
                    MessageBox.Show("ABUIOHDFSA");
                    downloadHandler.Abort(downloadHandler.Downloads[downloadsDataGridView.SelectedRows[0].Index]);
                }
            }
        }

        private void downloadsDataGridView_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            cancelDlButton.Enabled = true;
        }

        private void downloadsDataGridView_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void downloadsDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            //If a row is selected, leave the cancel button enabled
            if (downloadsDataGridView.SelectedRows.Count > 0)
            {
                cancelDlButton.Enabled = true;
            }
            else
            {
                cancelDlButton.Enabled = false;
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // If there are downloads running, ask user if they would like to close
            if (downloadHandler.Downloads.Count > 0)
            {
                DialogResult result = MessageBox.Show("Downloads are running. Are you sure you would like to exit?", "Downloads Running", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        private void optionsBtn_Click(object sender, EventArgs e)
        {
            // Show Options as Dialog
            OptionsForm optionsForm = new OptionsForm();
            optionsForm.ShowDialog();
        }

        private void resultDescription_TextChanged(object sender, EventArgs e)
        {

        }

        private void resultsGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void resultsGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Do nothing if header clicked.
            if (e.RowIndex == -1)
                return;

            // Run the downloadBtn_Click event
            downloadButton_Click(this, new EventArgs());
        }

        private void pagesLbl_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            // If value is not a multiple of 20 from 20-100, set it to the next lowest value
            if (resultsNum.Value % 20 != 0 && resultsNum.Value > 20 && resultsNum.Value < 100)
            {
                resultsNum.Value -= resultsNum.Value % 20;
            }
        }

        private void resultsGrid_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
        }

        private void downloadTab_Click(object sender, EventArgs e)
        {

        }

        private void openDownloadsBtn_Click(object sender, EventArgs e)
        {

            // Open explorer to text in Downloads directory Box
            try
            {
                Process.Start("explorer.exe", @dlDirTxtBox.Text);
            }
            catch
            {
                MessageBox.Show("Error 31: Opening directory failed. Directory may not exist or permissions may be incorrect.");
            }
        }

        private void controlTabs_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
