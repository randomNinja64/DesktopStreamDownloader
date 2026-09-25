using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace DesktopStreamDownloader
{

    public partial class MainForm : Form
    {
        DownloadHandler downloadHandler;

        private BackgroundWorker searchWorker;
        private BackgroundWorker thumbnailWorker;
        private string currentThumbnailId = "";
        private bool searchInProgress = false;
        private Dictionary<string, Image> thumbnailCache = new Dictionary<string, Image>();

        public MainForm()
        {
            InitializeComponent();
            downloadHandler = new DownloadHandler(this);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
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
            if (searchInProgress)
            {
                return;
            }

            // Switch To Search Tab
            controlTabs.SelectedTab = searchTab;

            // Clear Search Results
            resultsGrid.Rows.Clear();
            currentThumbnailId = "";
            SearchHandler.CancelThumbnail();
            ClearThumbnailCache();

            // Notify user that search is happening via info text box
            resultDescription.Text = "Grabbing Results. Please wait...";

            string query = searchTxtBox.Text;
            int count = Convert.ToInt32(resultsNum.Value);

            SetSearchControlsEnabled(false);
            searchInProgress = true;

            searchWorker = new BackgroundWorker();
            searchWorker.DoWork += searchWorker_DoWork;
            searchWorker.RunWorkerCompleted += searchWorker_RunWorkerCompleted;
            searchWorker.RunWorkerAsync(new object[] { query, count });
        }

        private void searchWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            object[] args = (object[])e.Argument;
            string query = (string)args[0];
            int count = (int)args[1];
            string errorMessage;
            List<SearchHandler.VideoItem> results = SearchHandler.Search(query, count, out errorMessage);
            e.Result = new object[] { results, errorMessage };
        }

        private void searchWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            searchInProgress = false;
            SetSearchControlsEnabled(true);
            resultDescription.Text = "";

            if (e.Error != null)
            {
                MessageBox.Show("Error 01: Error retrieving results. Please check your Internet connection or that yt-dlp is working correctly.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            object[] payload = e.Result as object[];
            if (payload == null)
            {
                return;
            }

            List<SearchHandler.VideoItem> results = payload[0] as List<SearchHandler.VideoItem>;
            string errorMessage = payload[1] as string;

            if (results == null)
            {
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return;
            }

            if (results.Count > 0)
            {
                foreach (SearchHandler.VideoItem result in results)
                {
                    resultsGrid.Rows.Add(result.title, result.identifier, result.FormatPreview(), result.views);
                }
            }
        }

        private void SetSearchControlsEnabled(bool enabled)
        {
            searchBtn.Enabled = enabled;
            searchTxtBox.Enabled = enabled;
            resultsNum.Enabled = enabled;
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

                // Download thumbnail asynchronously via curl
                BeginLoadThumbnail(identifier);

                // If description text box is blank, set it to "No description found."
                if (resultDescription.Text == "")
                {
                    resultDescription.Text = "No description found.";
                }

            }
        }

        private void BeginLoadThumbnail(string identifier)
        {
            currentThumbnailId = identifier;

            Image cached;
            if (thumbnailCache.TryGetValue(identifier, out cached))
            {
                resultPreview.Image = cached;
                return;
            }

            resultPreview.Image = null;
            resultPreview.ImageLocation = null;

            BackgroundWorker worker = new BackgroundWorker();
            thumbnailWorker = worker;
            worker.DoWork += thumbnailWorker_DoWork;
            worker.RunWorkerCompleted += thumbnailWorker_RunWorkerCompleted;
            worker.RunWorkerAsync(identifier);
        }

        private void thumbnailWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            string identifier = (string)e.Argument;
            Image image = SearchHandler.LoadThumbnail(identifier);
            e.Result = new object[] { identifier, image };
        }

        private void thumbnailWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null || e.Result == null)
            {
                return;
            }

            object[] parts = (object[])e.Result;
            string identifier = (string)parts[0];
            Image image = parts[1] as Image;

            if (image != null && !thumbnailCache.ContainsKey(identifier))
            {
                thumbnailCache[identifier] = image;
            }
            else if (image != null)
            {
                image.Dispose();
            }

            if (identifier != currentThumbnailId)
            {
                return;
            }

            Image shown;
            if (thumbnailCache.TryGetValue(identifier, out shown))
            {
                resultPreview.Image = shown;
            }
            else
            {
                resultPreview.Image = null;
                resultPreview.ImageLocation = null;
            }
        }

        private void ClearThumbnailCache()
        {
            resultPreview.Image = null;
            resultPreview.ImageLocation = null;
            foreach (Image image in thumbnailCache.Values)
            {
                image.Dispose();
            }
            thumbnailCache.Clear();
        }

        private void downloadButton_Click(object sender, EventArgs e)
        {
            //If an item is selected, Open DownloadForm and pass in the selected item's identifier
            if (resultsGrid.SelectedRows.Count > 0)
            {
                // Create filename (video title + .mp4)
                string filename = resultsGrid.SelectedRows[0].Cells[0].Value.ToString() + ".mp4";

                Uri URL = new Uri("https://youtube.com/watch?v=" + resultsGrid.SelectedRows[0].Cells[1].Value.ToString());

                // Set status label to indicate item added to queue
                queueStatusLbl.Text = "Added " + resultsGrid.SelectedRows[0].Cells[0].Value.ToString() + " to queue.";

                downloadHandler.addDownload(URL, filename);
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

        private void cancelDlButton_Click(object sender, EventArgs e)
        {
            // If no row is selected, disable cancel button and return
            if (downloadsDataGridView.SelectedRows.Count == 0)
            {
                cancelDlButton.Enabled = false;
                return;
            }

            int index = downloadsDataGridView.SelectedRows[0].Index;
            if (index < 0 || index >= downloadHandler.Downloads.Count)
            {
                return;
            }

            // Index 0 is active: Kill yt-dlp; Exited → OnDownloadCompleted removes it and starts the next.
            // Index > 0 is queued only: remove from the list.
            if (index == 0)
            {
                downloadHandler.Abort(downloadHandler.Downloads[0]);
            }
            else
            {
                downloadHandler.removeDownloadAtIndex(index);
            }
        }

        private void downloadsDataGridView_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            cancelDlButton.Enabled = true;
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
                    return;
                }

                downloadHandler.AbortActiveForExit();
            }

            SearchHandler.CancelThumbnail();
            ClearThumbnailCache();
        }

        private void optionsBtn_Click(object sender, EventArgs e)
        {
            // Show Options as Dialog
            OptionsForm optionsForm = new OptionsForm();
            optionsForm.ShowDialog();
        }

        private void resultsGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Do nothing if header clicked.
            if (e.RowIndex == -1)
                return;

            // Run the downloadBtn_Click event
            downloadButton_Click(this, new EventArgs());
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
                MessageBox.Show("Error 12: Opening directory failed. Directory may not exist or permissions may be incorrect.");
            }
        }
    }
}
