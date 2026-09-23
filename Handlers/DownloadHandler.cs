using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace DesktopStreamDownloader
{
    public class DownloadHandler
    {
        MainForm frm = MainForm.frmObj;
        public BindingList<Download> Downloads;
        private Process _activeYtDlpProcess;

        // Constructor
        public DownloadHandler()
        {
            this.Downloads = new BindingList<Download>();
        }

        // Function to Add Download
        public void addDownload(Uri downloadUrl, string fileName, System.Windows.Forms.Timer progressTimer)
        {
            // Correct filename, removing any invalid characters for Windows, replacing them with -
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                fileName = fileName.Replace(c, '-');
            }

            Downloads.Add(new Download(downloadUrl, fileName));

            // If progress timer isn't running, start it and start the first download
            if (!progressTimer.Enabled)
            {
                progressTimer.Start();
                downloadItem(Downloads[0], Properties.Settings.Default.DownloadPath);
            }
        }

        // Function to download item
        public void downloadItem(Download downloadItem, string destination)
        {

            // Create destination directory
            Directory.CreateDirectory(destination);

            // Download file
            //downloadItem.WebClient.DownloadFileAsync(downloadItem.downloadUrl, destination + "\\" + downloadItem.fileName);

            // Add Event Handlers For Progress and Completion
            //downloadItem.WebClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(webClient_DownloadProgressChanged);
            // Create async event handler for completion and pass current download into it
            //downloadItem.WebClient.DownloadFileCompleted += (sender, e) => webClient_DownloadFileCompleted(sender, e, downloadItem);

            string res = Properties.Settings.Default.DefaultQuality.Substring(0, Properties.Settings.Default.DefaultQuality.Length - 1); ;

            Process youtubedlprocess = new Process();
            youtubedlprocess.StartInfo.FileName = Path.Combine(Application.StartupPath, "yt-dlp.exe");
            youtubedlprocess.StartInfo.Arguments = $"-S res:{res},ext:mp4:m4a --recode mp4 -o \"" + destination + "\\" + downloadItem.fileName + "\" " + "\"" + downloadItem.downloadUrl.ToString() + "\"";
            youtubedlprocess.StartInfo.WorkingDirectory = Application.StartupPath;
            youtubedlprocess.StartInfo.UseShellExecute = false;
            youtubedlprocess.StartInfo.RedirectStandardOutput = true;
            youtubedlprocess.StartInfo.CreateNoWindow = true;
            youtubedlprocess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            youtubedlprocess.EnableRaisingEvents = true;
            youtubedlprocess.OutputDataReceived += (sender, e) => updateDLProgress(e.Data);


            youtubedlprocess.Start();
            _activeYtDlpProcess = youtubedlprocess;
            youtubedlprocess.BeginOutputReadLine();
            // Async event handler for when process exits
            youtubedlprocess.Exited += (sender, e) => OnDownloadCompleted();
        }

        public void updateDLProgress(string YTDLOutput)
        {
            string formattedOutput = ParseAndFormatOutput(YTDLOutput);
            Downloads[0].downloadProgress = formattedOutput;
            Console.WriteLine(YTDLOutput);
        }

        private string ParseAndFormatOutput(string output)
        {
            if (output != null)
            {
                // Regex to extract the percentage from the output string
                var match = Regex.Match(output, @"(\d{1,3}\.\d{1,2})%");

                if (match.Success)
                {
                    string percentage = match.Groups[1].Value;
                    return $"{percentage}";
                }
            }

            // If no percentage is found, return "Preparing..."
            return "Preparing...";
        }

        // The event that will fire whenever the progress of the WebClient is completed
        private void OnDownloadCompleted()
        {
            ClearActiveYtDlpProcess();

            // Messagebox to show download count
            //MessageBox.Show("Downloads remaining in queue: " + Downloads.Count);

            //Downloads.RemoveAt(0);
            if (Downloads.Count > 0)
            {
                // Messagebox, attempting to remove a download
                //MessageBox.Show("Attempting to remove a download");

                // Workaround for when form is closing
                // If no forms are open, return
                if (Application.OpenForms.Count == 0)
                {
                    return;
                }

                // Check if the current thread is the UI thread
                if (Application.OpenForms[0].InvokeRequired)
                {
                    // Use BeginInvoke to execute the downloadItem method on the UI thread
                    Application.OpenForms[0].BeginInvoke(new Action(() => Downloads.RemoveAt(0)));

                    // Wait for the above line to finish
                    Thread.Sleep(100);

                    // If there are still more downloads in the queue, start the next one
                    if (Downloads.Count > 0)
                    {
                        Application.OpenForms[0].BeginInvoke(new Action(() => downloadItem(Downloads[0], Properties.Settings.Default.DownloadPath)));
                    }

                    // If there are no more downloads in the queue, stop the timer
                    else
                    {
                        MainForm.frmObj.progressTimer.Stop();
                    }
                }
                else
                {
                    Downloads.RemoveAt(0);
                    // If there are still more downloads in the queue, start the next one
                    if (Downloads.Count > 0)
                    {
                        downloadItem(Downloads[0], Properties.Settings.Default.DownloadPath);
                    }
                }
            }
            else
            {
                // Stop the timer
                MainForm.frmObj.progressTimer.Stop();
            }
        }

        // Function to abort download
        public void Abort(Download downloadToAbort)
        {
            // Kill only the yt-dlp process started by this handler
            try
            {
                if (_activeYtDlpProcess != null && !_activeYtDlpProcess.HasExited)
                {
                    _activeYtDlpProcess.Kill();
                }
            }
            catch (InvalidOperationException)
            {
                // Process already exited or was never started
            }
            catch (Win32Exception)
            {
                // Process could not be terminated
            }

            string filePath = Properties.Settings.Default.DownloadPath + downloadToAbort.fileName;

            try
            {
                // Check if the file exists
                if (File.Exists(filePath))
                {
                    // Attempt to delete the file
                    File.Delete(filePath);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while attempting to delete the file '{filePath}': {ex.Message}");
            }
        }

        private void ClearActiveYtDlpProcess()
        {
            if (_activeYtDlpProcess != null)
            {
                _activeYtDlpProcess.Dispose();
                _activeYtDlpProcess = null;
            }
        }

        public void removeDownloadAtIndex(int index)
        {
            Downloads.RemoveAt(index);
        }
    }
}
