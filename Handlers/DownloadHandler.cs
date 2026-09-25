using System;
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
        private static readonly Regex progressRegex = new Regex(@"(\d{1,3}\.\d{1,2})%");
        private static readonly Regex speedRegex = new Regex(@"at\s+(\S+/s)");
        private string _progressStage = "Preparing...";
        private string _progressText = "";

        MainForm frm;
        public BindingList<Download> Downloads;
        private Process _activeYtDlpProcess;
        private StringBuilder _ytDlpError;

        // Constructor
        public DownloadHandler(MainForm form)
        {
            frm = form;
            this.Downloads = new BindingList<Download>();
        }

        // Function to Add Download
        public void addDownload(Uri downloadUrl, string fileName)
        {
            // Correct filename, removing any invalid characters for Windows, replacing them with -
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                fileName = fileName.Replace(c, '-');
            }

            Downloads.Add(new Download(downloadUrl, fileName));

            // First item starts immediately. Later items wait until the active one finishes.
            if (Downloads.Count == 1)
            {
                downloadItem(Downloads[0], Properties.Settings.Default.DownloadPath);
            }
        }

        // Function to download item
        public void downloadItem(Download downloadItem, string destination)
        {

            // Create destination directory
            Directory.CreateDirectory(destination);

            string res = Properties.Settings.Default.DefaultQuality.Substring(0, Properties.Settings.Default.DefaultQuality.Length - 1); ;

            Process youtubedlprocess = new Process();
            youtubedlprocess.StartInfo.FileName = Path.Combine(Application.StartupPath, "yt-dlp.exe");
            youtubedlprocess.StartInfo.Arguments = $"-S res:{res},vcodec:h264,acodec:aac,ext:mp4:m4a --recode mp4 -o \"" + destination + "\\" + downloadItem.fileName + "\" " + "\"" + downloadItem.downloadUrl.ToString() + "\"";
            youtubedlprocess.StartInfo.WorkingDirectory = Application.StartupPath;
            youtubedlprocess.StartInfo.UseShellExecute = false;
            youtubedlprocess.StartInfo.RedirectStandardOutput = true;
            youtubedlprocess.StartInfo.RedirectStandardError = true;
            youtubedlprocess.StartInfo.CreateNoWindow = true;
            youtubedlprocess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            youtubedlprocess.EnableRaisingEvents = true;
            youtubedlprocess.OutputDataReceived += (sender, e) => updateDLProgress(e.Data);
            youtubedlprocess.ErrorDataReceived += (sender, e) => AppendYtDlpError(e.Data);

            _ytDlpError = new StringBuilder();
            _progressStage = "Preparing...";
            _progressText = "";

            youtubedlprocess.Start();
            _activeYtDlpProcess = youtubedlprocess;
            youtubedlprocess.BeginOutputReadLine();
            youtubedlprocess.BeginErrorReadLine();
            // Async event handler for when process exits
            youtubedlprocess.Exited += (sender, e) => OnDownloadCompleted();
        }

        public void updateDLProgress(string YTDLOutput)
        {
            if (YTDLOutput == null || Application.OpenForms.Count == 0)
            {
                return;
            }

            ParseAndFormatOutput(YTDLOutput);
            string stage = _progressStage;
            string progress = _progressText;

            // OutputDataReceived is not the UI thread; BindingList must be updated there.
            // Count is rechecked because cancel/completion may RemoveAt(0) first.
            Action apply = () =>
            {
                if (Downloads.Count > 0)
                {
                    Downloads[0].downloadStatus = stage;
                    Downloads[0].downloadProgress = progress;
                }
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

        private void ParseAndFormatOutput(string output)
        {
            if (output == null)
            {
                return;
            }

            string stage = StageFromLine(output);
            if (stage != null)
            {
                _progressStage = stage;
            }

            Match match = progressRegex.Match(output);
            if (!match.Success)
            {
                return;
            }

            _progressText = match.Groups[1].Value + "%";
            Match speed = speedRegex.Match(output);
            if (speed.Success && speed.Groups[1].Value.IndexOf("Unknown") < 0)
            {
                _progressText = _progressText + " · " + speed.Groups[1].Value;
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

        private void AppendYtDlpError(string line)
        {
            if (line != null && _ytDlpError != null)
            {
                _ytDlpError.AppendLine(line);
            }
        }

        private void OnDownloadCompleted()
        {
            int exitCode = 0;
            string errorText = ErrorLines(_ytDlpError);
            string failedName = Downloads.Count > 0 ? Downloads[0].fileName : "download";

            try
            {
                if (_activeYtDlpProcess != null && _activeYtDlpProcess.HasExited)
                {
                    exitCode = _activeYtDlpProcess.ExitCode;
                }
            }
            catch (InvalidOperationException)
            {
            }

            ClearActiveYtDlpProcess();

            if (Application.OpenForms.Count == 0)
            {
                return;
            }

            // Remove finished item and start the next one on the UI thread.
            Action advance = () =>
            {
                if (exitCode != 0 && errorText != "")
                {
                    if (errorText.Length > 800)
                    {
                        errorText = errorText.Substring(errorText.Length - 800);
                    }
                    MessageBox.Show(errorText, "Download failed: " + failedName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (exitCode == 0)
                {
                    frm.queueStatusLbl.Text = "Completed " + failedName + ".";
                }

                if (Downloads.Count > 0)
                {
                    Downloads.RemoveAt(0);
                }

                if (Downloads.Count > 0)
                {
                    downloadItem(Downloads[0], Properties.Settings.Default.DownloadPath);
                }
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
            if (Downloads.Count == 0)
            {
                return;
            }

            if (_ytDlpError != null)
            {
                _ytDlpError.Length = 0;
            }

            Download active = Downloads[0];
            Downloads.Clear();
            Abort(active);
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

            string filePath = Path.Combine(Properties.Settings.Default.DownloadPath, downloadToAbort.fileName);

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
