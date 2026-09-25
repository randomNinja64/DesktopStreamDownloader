using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace DesktopStreamDownloader
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string missingDependencies = MissingDependencyMessage();
            if (missingDependencies != null)
            {
                MessageBox.Show(missingDependencies, "Missing dependencies", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Application.Run(new MainForm());
        }

        private static string MissingDependencyMessage()
        {
            string[] required = new string[] { "yt-dlp.exe", "curl.exe", "ffmpeg.exe" };
            StringBuilder missing = new StringBuilder();
            foreach (string name in required)
            {
                if (!File.Exists(Path.Combine(Application.StartupPath, name)))
                {
                    missing.AppendLine(name);
                }
            }

            if (missing.Length == 0)
            {
                return null;
            }

            return "These files must be in the same folder as the application:" + Environment.NewLine + Environment.NewLine + missing.ToString().TrimEnd();
        }
    }
}
