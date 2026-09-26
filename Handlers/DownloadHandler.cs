using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace DesktopStreamDownloader
{
    public class DownloadHandler
    {
        private class RunningDownload
        {
            public Download Item;
            public Process Process;
            public StringBuilder Error = new StringBuilder();
            public string Stage = "Preparing...";
            public string Progress = "";
            public bool Cancelled;
            public bool Active = true;
        }

        private static readonly Regex progressRegex = new Regex(@"(\d{1,3}\.\d{1,2})%");
        private static readonly Regex speedRegex = new Regex(@"at\s+(\S+/s)");

        MainForm frm;
        public BindingList<Download> Downloads;
        private List<RunningDownload> _running = new List<RunningDownload>();

        // Constructor
        public DownloadHandler(MainForm form)
        {
            frm = form;
            Downloads = new BindingList<Download>();
        }

        // Function to Add Download. Returns false when the user declines an overwrite.
        public bool addDownload(Uri downloadUrl, string fileName)
        {
            // Correct filename, removing any invalid characters for Windows, replacing them with -
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                fileName = fileName.Replace(c, '-');
            }

            bool overwrite = false;
            if (TitleCollision(fileName))
            {
                DialogResult result = MessageBox.Show(
                    "\"" + fileName + "\" is already in the download folder or the queue. Overwrite it?",
                    "Overwrite file",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
                if (result != DialogResult.Yes)
                {
                    return false;
                }
                overwrite = true;
            }

            Downloads.Add(new Download(downloadUrl, fileName, overwrite));
            FillSlots();
            return true;
        }

        public void FillSlots()
        {
            string destination = Properties.Settings.Default.DownloadPath;
            while (_running.Count < Properties.Settings.Default.MaxConcurrentDownloads)
            {
                Download next = NextQueued();
                if (next == null)
                {
                    return;
                }
                downloadItem(next, destination);
            }
        }

        private Download NextQueued()
        {
            foreach (Download download in Downloads)
            {
                if (!FileNameRunning(download.fileName))
                {
                    return download;
                }
            }
            return null;
        }

        private bool FileNameRunning(string fileName)
        {
            foreach (RunningDownload job in _running)
            {
                if (string.Equals(job.Item.fileName, fileName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        private bool TitleCollision(string fileName)
        {
            string path = Path.Combine(Properties.Settings.Default.DownloadPath, fileName);
            if (File.Exists(path))
            {
                return true;
            }

            foreach (Download download in Downloads)
            {
                if (string.Equals(download.fileName, fileName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        // Function to download item
        public void downloadItem(Download downloadItem, string destination)
        {
            // Create destination directory
            Directory.CreateDirectory(destination);

            string res = Properties.Settings.Default.DefaultQuality.Substring(0, Properties.Settings.Default.DefaultQuality.Length - 1);
            string forceOverwrite = downloadItem.overwrite ? "--force-overwrites " : "";

            RunningDownload job = new RunningDownload();
            job.Item = downloadItem;

            Process youtubedlprocess = new Process();
            job.Process = youtubedlprocess;
            youtubedlprocess.StartInfo.FileName = Path.Combine(Application.StartupPath, "yt-dlp.exe");
            youtubedlprocess.StartInfo.Arguments = $"-S res:{res},vcodec:h264,acodec:aac,ext:mp4:m4a " + forceOverwrite + "--recode mp4 -o \"" + destination + "\\" + downloadItem.fileName + "\" " + "\"" + downloadItem.downloadUrl.ToString() + "\"";
            youtubedlprocess.StartInfo.WorkingDirectory = Application.StartupPath;
            youtubedlprocess.StartInfo.UseShellExecute = false;
            youtubedlprocess.StartInfo.RedirectStandardOutput = true;
            youtubedlprocess.StartInfo.RedirectStandardError = true;
            youtubedlprocess.StartInfo.CreateNoWindow = true;
            youtubedlprocess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            youtubedlprocess.EnableRaisingEvents = true;
            youtubedlprocess.OutputDataReceived += (sender, e) => updateDLProgress(job, e.Data);
            youtubedlprocess.ErrorDataReceived += (sender, e) => AppendYtDlpError(job, e.Data);
            youtubedlprocess.Exited += (sender, e) => OnDownloadCompleted(job);

            _running.Add(job);

            try
            {
                youtubedlprocess.Start();
                youtubedlprocess.BeginOutputReadLine();
                youtubedlprocess.BeginErrorReadLine();
            }
            catch
            {
                RemoveRunning(job);
                throw;
            }
        }

        private void updateDLProgress(RunningDownload job, string YTDLOutput)
        {
            if (YTDLOutput == null || !job.Active || Application.OpenForms.Count == 0)
            {
                return;
            }

            ParseAndFormatOutput(job, YTDLOutput);
            string stage = job.Stage;
            string progress = job.Progress;
            Download item = job.Item;

            // OutputDataReceived is not the UI thread; BindingList must be updated there.
            Action apply = () =>
            {
                if (!job.Active)
                {
                    return;
                }

                item.downloadStatus = stage;
                item.downloadProgress = progress;
            };

            Form form = Application.OpenForms[0];
            if (form.InvokeRequired)
            {
                form.BeginInvoke(apply);
            }
            else
            {
                apply();
            }
        }

        private void ParseAndFormatOutput(RunningDownload job, string output)
        {
            if (output == null)
            {
                return;
            }

            string stage = StageFromLine(output);
            if (stage != null)
            {
                job.Stage = stage;
            }

            Match match = progressRegex.Match(output);
            if (!match.Success)
            {
                return;
            }

            job.Progress = match.Groups[1].Value + "%";
            Match speed = speedRegex.Match(output);
            if (speed.Success && speed.Groups[1].Value.IndexOf("Unknown") < 0)
            {
                job.Progress = job.Progress + " · " + speed.Groups[1].Value;
            }
        }

        private static string StageFromLine(string output)
        {
            if (output.IndexOf("[Merger]") >= 0 || output.IndexOf("Merging formats") >= 0)
            {
                return "Combining";
            }
            if (output.IndexOf("[VideoConvertor]") >= 0 || output.IndexOf("Converting video") >= 0)
            {
                return "Converting";
            }
            if (output.IndexOf("Remuxing") >= 0)
            {
                return "Remuxing";
            }
            if (output.IndexOf("[ExtractAudio]") >= 0)
            {
                return "Extracting audio";
            }
            if (output.IndexOf("Destination:") >= 0)
            {
                string lower = output.ToLower();
                if (lower.IndexOf(".m4a") >= 0 || lower.IndexOf(".aac") >= 0 || lower.IndexOf(".mp3") >= 0 || lower.IndexOf(".opus") >= 0
                    || lower.IndexOf(".f249.") >= 0 || lower.IndexOf(".f250.") >= 0 || lower.IndexOf(".f251.") >= 0)
                {
                    return "Downloading audio";
                }
                return "Downloading video";
            }
            return null;
        }

        private static string ErrorLines(StringBuilder stderr)
        {
            if (stderr == null)
            {
                return "";
            }

            StringBuilder errors = new StringBuilder();
            string[] lines = stderr.ToString().Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines)
            {
                if (line.IndexOf("ERROR:") < 0)
                {
                    continue;
                }
                if (errors.Length > 0)
                {
                    errors.AppendLine();
                }
                errors.Append(line.Trim());
            }

            string text = errors.ToString();
            if (text.Length > 800)
            {
                text = text.Substring(text.Length - 800);
            }
            return text;
        }

        private static void AppendYtDlpError(RunningDownload job, string line)
        {
            if (line != null && job.Error != null)
            {
                job.Error.AppendLine(line);
            }
        }

        private void OnDownloadCompleted(RunningDownload job)
        {
            if (!job.Active)
            {
                return;
            }

            job.Active = false;
            int exitCode = 0;
            bool cancelled = job.Cancelled;
            string errorText = ErrorLines(job.Error);
            string finishedName = job.Item != null ? job.Item.fileName : "download";

            try
            {
                if (job.Process != null && job.Process.HasExited)
                {
                    exitCode = job.Process.ExitCode;
                }
            }
            catch (InvalidOperationException)
            {
            }

            if (Application.OpenForms.Count == 0)
            {
                RemoveRunning(job);
                return;
            }

            // Remove finished item and start the next one on the UI thread.
            Action advance = () =>
            {
                RemoveRunning(job);

                if (cancelled)
                {
                    frm.queueStatusLbl.Text = "Cancelled " + finishedName + ".";
                }
                else if (exitCode != 0 && errorText != "")
                {
                    MessageBox.Show(errorText, "Download failed: " + finishedName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (exitCode == 0)
                {
                    frm.queueStatusLbl.Text = "Completed " + finishedName + ".";
                }

                RemoveDownload(job.Item);
                FillSlots();
            };

            Form form = Application.OpenForms[0];
            if (form.InvokeRequired)
            {
                form.BeginInvoke(advance);
            }
            else
            {
                advance();
            }
        }

        public void AbortActiveForExit()
        {
            if (Downloads.Count == 0 && _running.Count == 0)
            {
                return;
            }

            List<RunningDownload> jobs = new List<RunningDownload>(_running);
            foreach (RunningDownload job in jobs)
            {
                job.Cancelled = true;
            }

            Downloads.Clear();
            foreach (RunningDownload job in jobs)
            {
                KillJob(job);
            }
        }

        public void Cancel(Download download)
        {
            RunningDownload job = FindRunning(download);
            if (job == null)
            {
                RemoveDownload(download);
                return;
            }

            KillJob(job);
        }

        private void KillJob(RunningDownload job)
        {
            // The flag is set only while that process is alive, so a late click cannot mark a finished download as cancelled.
            // Exit sets Cancelled on every job before calling this.
            try
            {
                if (job.Process != null && !job.Process.HasExited)
                {
                    job.Cancelled = true;
                    job.Process.Kill();
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

            string filePath = Path.Combine(Properties.Settings.Default.DownloadPath, job.Item.fileName);

            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while attempting to delete the file '{filePath}': {ex.Message}");
            }
        }

        private RunningDownload FindRunning(Download download)
        {
            foreach (RunningDownload job in _running)
            {
                if (job.Item == download)
                {
                    return job;
                }
            }
            return null;
        }

        private void RemoveRunning(RunningDownload job)
        {
            job.Active = false;
            _running.Remove(job);
            if (job.Process != null)
            {
                job.Process.Dispose();
                job.Process = null;
            }
        }

        private void RemoveDownload(Download download)
        {
            if (download == null)
            {
                return;
            }

            Downloads.Remove(download);
        }
    }
}
