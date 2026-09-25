using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace DesktopStreamDownloader
{
    internal class SearchHandler
    {
        public struct VideoItem
        {
            public string title;
            public string description;
            public string identifier;
            public string author;
            public string published;
            public string length;
            public Int64 views;

            public string FormatPreview()
            {
                string preview = "Author: " + author;

                if (description != null && description.Trim() != "")
                {
                    preview = preview + Environment.NewLine + Environment.NewLine + description.Trim();
                }

                return preview;
            }
        }

        /// <summary>
        /// Searches via yt-dlp. On failure returns null and sets errorMessage (caller should show UI).
        /// </summary>
        public static List<VideoItem> Search(string query, int resultsNum, out string errorMessage)
        {
            errorMessage = null;

            if (query == null || query.Trim() == "")
            {
                errorMessage = "Error 02: Search queries cannot be blank.";
                return null;
            }

            if (resultsNum < 1)
            {
                resultsNum = 1;
            }
            else if (resultsNum > 200)
            {
                resultsNum = 200;
            }

            string ytDlpPath = Path.Combine(Application.StartupPath, "yt-dlp.exe");
            if (!File.Exists(ytDlpPath))
            {
                errorMessage = "Error 01: yt-dlp.exe was not found. Please ensure it is placed in the same folder as the application.";
                return null;
            }

            string escapedQuery = query.Replace("\"", "\\\"");
            string searchArg = "ytsearch" + resultsNum + ":" + escapedQuery;

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = ytDlpPath,
                Arguments = "-j --flat-playlist --no-warnings --extractor-args \"youtubetab:approximate_date\" \"" + searchArg + "\"",
                WorkingDirectory = Application.StartupPath,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            string stdout;
            int exitCode;
            try
            {
                using (Process process = Process.Start(startInfo))
                {
                    process.ErrorDataReceived += delegate { };
                    process.BeginErrorReadLine();
                    stdout = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();
                    exitCode = process.ExitCode;
                }
            }
            catch
            {
                errorMessage = "Error 01: Error retrieving results. Please check your Internet connection or that yt-dlp is working correctly.";
                return null;
            }

            List<VideoItem> results = new List<VideoItem>();
            HashSet<string> seenIds = new HashSet<string>();

            if (string.IsNullOrEmpty(stdout))
            {
                if (exitCode != 0)
                {
                    errorMessage = "Error 01: Error retrieving results. Please check your Internet connection or that yt-dlp is working correctly.";
                    return null;
                }

                errorMessage = "Error 03: No results found.";
                return null;
            }

            try
            {
                using (StringReader reader = new StringReader(stdout))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.Trim() == "")
                        {
                            continue;
                        }

                        JObject item = JObject.Parse(line);

                        string identifier = TokenAsString(item["id"]);
                        if (identifier == "")
                        {
                            continue;
                        }

                        if (seenIds.Contains(identifier))
                        {
                            continue;
                        }

                        string liveStatus = TokenAsString(item["live_status"]);
                        if (liveStatus == "is_live" || liveStatus == "is_upcoming")
                        {
                            continue;
                        }

                        Int64 views = 0;
                        if (item["view_count"] != null && item["view_count"].Type != JTokenType.Null)
                        {
                            views = (Int64)item["view_count"];
                        }

                        string author = TokenAsString(item["channel"]);
                        if (author == "")
                        {
                            author = TokenAsString(item["uploader"]);
                        }
                        if (author == "")
                        {
                            author = "Unknown";
                        }

                        string published = "Unknown";
                        string uploadDate = TokenAsString(item["upload_date"]);
                        if (uploadDate.Length == 8)
                        {
                            published = uploadDate.Substring(0, 4) + "-" + uploadDate.Substring(4, 2) + "-" + uploadDate.Substring(6, 2);
                        }

                        string lengthText = "Unknown";
                        if (item["duration"] != null && item["duration"].Type != JTokenType.Null)
                        {
                            double seconds = (double)item["duration"];
                            TimeSpan videoLength = TimeSpan.FromSeconds(seconds);
                            if (videoLength.TotalHours >= 1)
                            {
                                lengthText = string.Format("{0}:{1:D2}:{2:D2}", (int)videoLength.TotalHours, videoLength.Minutes, videoLength.Seconds);
                            }
                            else
                            {
                                lengthText = string.Format("{0:D2}:{1:D2}", videoLength.Minutes, videoLength.Seconds);
                            }
                        }
                        else
                        {
                            string durationString = TokenAsString(item["duration_string"]);
                            if (durationString != "")
                            {
                                lengthText = durationString;
                            }
                        }

                        VideoItem result = new VideoItem
                        {
                            title = TokenAsString(item["title"]),
                            identifier = identifier,
                            author = author,
                            published = published,
                            length = lengthText,
                            views = views,
                            description = TokenAsString(item["description"]).Trim()
                        };

                        results.Add(result);
                        seenIds.Add(identifier);
                    }
                }
            }
            catch
            {
                errorMessage = "Error 01: Error retrieving results. Please check your Internet connection or that yt-dlp is working correctly.";
                return null;
            }

            if (results.Count == 0)
            {
                errorMessage = "Error 03: No results found.";
                return null;
            }

            return results;
        }

        private static Process thumbnailProcess;
        private static readonly object thumbnailLock = new object();

        public static void CancelThumbnail()
        {
            Process previous;
            lock (thumbnailLock)
            {
                previous = thumbnailProcess;
                thumbnailProcess = null;
            }
            KillProcess(previous);
        }

        public static Image LoadThumbnail(string identifier)
        {
            if (identifier == null || identifier == "")
            {
                return null;
            }

            string curlPath = Path.Combine(Application.StartupPath, "curl.exe");
            if (!File.Exists(curlPath))
            {
                return null;
            }

            string url = "https://i.ytimg.com/vi/" + identifier + "/mqdefault.jpg";
            string arguments = "-sL \"" + url + "\"";
            string caBundle = Path.Combine(Application.StartupPath, "curl-ca-bundle.crt");
            if (File.Exists(caBundle))
            {
                arguments = "--cacert \"" + caBundle + "\" " + arguments;
            }

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = curlPath,
                Arguments = arguments,
                WorkingDirectory = Application.StartupPath,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            Process process = null;
            try
            {
                process = Process.Start(startInfo);
                Process previous;
                lock (thumbnailLock)
                {
                    previous = thumbnailProcess;
                    thumbnailProcess = process;
                }
                KillProcess(previous);

                using (MemoryStream ms = new MemoryStream())
                {
                    CopyStream(process.StandardOutput.BaseStream, ms);
                    process.WaitForExit();

                    if (process.ExitCode != 0 || ms.Length == 0)
                    {
                        return null;
                    }

                    ms.Position = 0;
                    using (Image temp = Image.FromStream(ms))
                    {
                        return new Bitmap(temp);
                    }
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                if (process != null)
                {
                    lock (thumbnailLock)
                    {
                        if (thumbnailProcess == process)
                        {
                            thumbnailProcess = null;
                        }
                    }
                    process.Dispose();
                }
            }
        }

        private static void KillProcess(Process process)
        {
            try
            {
                if (process != null)
                {
                    process.Kill();
                }
            }
            catch
            {
            }
        }

        private static string TokenAsString(JToken token)
        {
            if (token == null || token.Type == JTokenType.Null)
            {
                return "";
            }
            return token.ToString();
        }

        private static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[8192];
            int read;
            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, read);
            }
        }
    }
}
